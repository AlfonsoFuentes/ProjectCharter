using Server.Database.Entities;
using Server.Database.Entities.PurchaseOrders;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Enums.TaskStatus;
using Shared.Models.Suppliers.Records;
using Shared.Models.Suppliers.Responses;
using Shared.Models.Warnings.Records;
using Shared.Models.Warnings.Responses;

namespace Server.EndPoint.Warnings.Queries
{
    public static class GetAllWarningsEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Warnings.EndPoint.GetAll, async (WarningGetAll request, IQueryRepository Repository) =>
                {


                    WarningResponseList response = new WarningResponseList();

                    response.Items.AddRange(await WarningFromPurchaseOrders(Repository));

                    await GetWarnings(response, Repository);

                    return Result.Success(response);

                });
            }
            async Task<List<WarningResponse>> WarningFromPurchaseOrders(IQueryRepository Repository)
            {
                DateTime now = DateTime.Now;
                Expression<Func<PurchaseOrder, bool>> Criteria = x =>
                    x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Approved.Id || x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Receiving.Id;

                Func<IQueryable<PurchaseOrder>, IIncludableQueryable<PurchaseOrder, object>> Includes = x => x
                .Include(x => x.Project)
                .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.BudgetItem!)
                //.Include(p => p.PurchaseOrderItems).ThenInclude(x => x.BasicEngineeringItem!)
                .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                .Include(x => x.Supplier!);
                var cache = StaticClass.PurchaseOrders.Cache.GetAllApproved;
                var PurchaseOrderRows = await Repository.GetAllAsync(cache, Criteria: Criteria, Includes: Includes);
                if (PurchaseOrderRows == null || PurchaseOrderRows.Count == 0) return new();
                PurchaseOrderRows = PurchaseOrderRows.Where(x => x.ExpectedDate <= now).ToList();
                return PurchaseOrderRows.Select(x => x.MapWarning()).ToList();
            }
            List<WarningResponse> WarningFromGanttTaskDelayedTask(List<Project> projects)
            {
                DateTime now = DateTime.Now;
                var cache = StaticClass.DeliverableGanttTasks.Cache.GetAllProjects;


                var deliverables = projects.SelectMany(x => x.Deliverables).ToList();
                var ganttTasks = deliverables.SelectMany(x => x.NewGanttTasks).ToList();

                var gantTasksFiltered = ganttTasks.Where(x => x.TaskStatus == GanttTaskStatusEnum.NotInitiated.Id && x.SubTasks.Count == 0).ToList();

                var gantTasksFilteredReal = gantTasksFiltered.Where(x => x.RealStartDate.HasValue && x.RealStartDate <= now).ToList();
                var gantTasksFilteredNotReal = gantTasksFiltered.Where(x => !x.RealStartDate.HasValue && x.StartDate <= now).ToList();


                var gantTasksResult = gantTasksFilteredReal.Concat(gantTasksFilteredNotReal).ToList();

                return gantTasksResult.Select(x => x.MapWarningDelayed()).ToList();
            }
            List<WarningResponse> WarningFromGanttTaskNotClosed(List<Project> projects)
            {
                DateTime now = DateTime.Now;

                var deliverables = projects.SelectMany(x => x.Deliverables).ToList();
                var ganttTasks = deliverables.SelectMany(x => x.NewGanttTasks).ToList();

                var gantTasksFiltered = ganttTasks.Where(x => x.TaskStatus == GanttTaskStatusEnum.OnGoing.Id && x.SubTasks.Count == 0).ToList();

                var gantTasksFilteredReal = gantTasksFiltered.Where(x => x.RealEndDate.HasValue && x.RealEndDate <= now).ToList();
                var gantTasksFilteredNotReal = gantTasksFiltered.Where(x => !x.RealEndDate.HasValue && x.EndDate <= now).ToList();


                var gantTasksResult = gantTasksFilteredReal.Concat(gantTasksFilteredNotReal).ToList();

                return gantTasksResult.Select(x => x.MapWarningNotClosed()).ToList();
            }
            async Task GetWarnings(WarningResponseList response, IQueryRepository Repository)
            {

                var cache = StaticClass.DeliverableGanttTasks.Cache.GetAllProjects;

                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x
                  .Include(x => x.Deliverables).ThenInclude(x => x.NewGanttTasks).ThenInclude(x => x.SubTasks)
               ;
                Expression<Func<Project, bool>> criteria = x => x.Status == ProjectStatusEnum.Approved.Id;
                var projects = await Repository.GetAllAsync(Cache: cache,Criteria:criteria, Includes: includes);
                if (projects == null) return;

                response.Items.AddRange(WarningFromGanttTaskDelayedTask(projects));
                response.Items.AddRange(WarningFromGanttTaskNotClosed(projects));

            }
        }
    }
}