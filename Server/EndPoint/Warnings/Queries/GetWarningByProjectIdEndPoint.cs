using Server.Database.Entities.PurchaseOrders;
using Server.EndPoint.MonitoringTasks.Queries;
using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Enums.TaskStatus;
using Shared.Models.Warnings.Records;
using Shared.Models.Warnings.Responses;

namespace Server.EndPoint.Suppliers.Queries
{
    public static class GetWarningByProjectIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Warnings.EndPoint.GetById, async (WarningGetByProjectId request, IQueryRepository Repository) =>
                {
                    WarningResponseList response = new WarningResponseList();

                    response.Items.AddRange(await WarningFromPurchaseOrders(request, Repository));
                    await GetWarnings(response, request, Repository);
           

                    return Result.Success(response);

                });
            }
            async Task<List<WarningResponse>> WarningFromPurchaseOrders(WarningGetByProjectId request, IQueryRepository Repository)
            {
                DateTime now = DateTime.Now;
                Expression<Func<PurchaseOrder, bool>> Criteria = x => x.ProjectId == request.ProjectId &&
                    (x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Approved.Id || x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Receiving.Id);

                Func<IQueryable<PurchaseOrder>, IIncludableQueryable<PurchaseOrder, object>> Includes = x => x
                .Include(x => x.Project)
                .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.BudgetItem!)
                //.Include(p => p.PurchaseOrderItems).ThenInclude(x => x.BasicEngineeringItem!)
                .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                .Include(x => x.Supplier!);
                var cache = StaticClass.PurchaseOrders.Cache.GetAllApprovedByProject(request.ProjectId);
                var PurchaseOrderRows = await Repository.GetAllAsync(cache, Criteria: Criteria, Includes: Includes);

                if (PurchaseOrderRows == null || PurchaseOrderRows.Count == 0) return new();
                PurchaseOrderRows = PurchaseOrderRows.Where(x => x.ExpectedDate <= now).ToList();
                return PurchaseOrderRows.Select(x => x.MapWarning()).ToList();
            }
            List<WarningResponse> WarningFromGanttTaskDelayedTask(Project project)
            {
                DateTime now = DateTime.Now;
                var gantTasksFiltered = project.Deliverables.SelectMany(x => x.NewGanttTasks.Where(x => x.TaskStatus == GanttTaskStatusEnum.NotInitiated.Id && x.SubTasks.Count == 0)).ToList();

                var gantTasksFilteredReal = gantTasksFiltered.Where(x => x.RealStartDate.HasValue && x.RealStartDate <= now).ToList();
                var gantTasksFilteredNotReal = gantTasksFiltered.Where(x => !x.RealStartDate.HasValue && x.StartDate <= now).ToList();


                var gantTasksResult = gantTasksFilteredReal.Concat(gantTasksFilteredNotReal).ToList();

                return gantTasksResult.Select(x => x.MapWarningDelayed()).ToList();
            }
            List<WarningResponse> WarningFromGanttTaskNotClosed(Project project)
            {
                DateTime now = DateTime.Now;

                var gantTasksFiltered = project.Deliverables.SelectMany(x => x.NewGanttTasks.Where(x => x.TaskStatus == GanttTaskStatusEnum.OnGoing.Id && x.SubTasks.Count == 0)).ToList();

                var gantTasksFilteredReal = gantTasksFiltered.Where(x => x.RealEndDate.HasValue && x.RealEndDate <= now).ToList();
                var gantTasksFilteredNotReal = gantTasksFiltered.Where(x => !x.RealEndDate.HasValue && x.EndDate <= now).ToList();



                var gantTasksResult = gantTasksFilteredReal.Concat(gantTasksFilteredNotReal).ToList();

                return gantTasksResult.Select(x => x.MapWarningNotClosed()).ToList();
            }

            async Task GetWarnings(WarningResponseList response, WarningGetByProjectId request, IQueryRepository Repository)
            {

                var cache = StaticClass.DeliverableGanttTasks.Cache.GetAll(request.ProjectId);

                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x
                  .Include(x => x.Deliverables).ThenInclude(x => x.NewGanttTasks).ThenInclude(x => x.SubTasks)
               ;
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                var project = await Repository.GetAsync(Cache: cache, Criteria: criteria, Includes: includes);
                if (project == null) return;

                response.Items.AddRange(WarningFromGanttTaskDelayedTask(project));
                response.Items.AddRange(WarningFromGanttTaskNotClosed(project));

            }
        }
        public static WarningResponse MapWarning(this PurchaseOrder purchaseOrder)
        {
            return new WarningResponse
            {
                PurchaseOrder = purchaseOrder.Map(),
                WarningText = $"Purchase Order {purchaseOrder.PONumber} in {purchaseOrder.Project.Name} Name: {purchaseOrder.PurchaseorderName} Supplier: {purchaseOrder.SupplierName} is overdue."
                ,
                ProjectName = purchaseOrder.Project.Name,
                ProjectNumber = $"CEC0000{purchaseOrder.Project.ProjectNumber}"

            };
        }
        public static WarningResponse MapWarningDelayed(this NewGanttTask task)
        {
            var startDate = task.RealStartDate.HasValue ? task.RealStartDate.Value : task.StartDate;
            return new WarningResponse
            {
                GanttTask = task.MapMonitoringParallel(),
                WarningText = $"Task {task.Name} in {task.Deliverable.Project.Name} is delayed. It was supposed to start on {startDate.ToShortDateString()}."
                 ,
                ProjectName = task.Deliverable.Project.Name,
                ProjectNumber = $"CEC0000{task.Deliverable.Project.ProjectNumber}"
            };
        }
        public static WarningResponse MapWarningNotClosed(this NewGanttTask task)
        {
            var endDate = task.RealEndDate.HasValue ? task.RealEndDate.Value : task.EndDate;
            return new WarningResponse
            {
                GanttTask = task.MapMonitoringParallel(),
                WarningText = $"Task {task.Name} in {task.Deliverable.Project.Name} is not closed. It was supposed to end on {endDate.ToShortDateString()}."
                 ,
                ProjectName = task.Deliverable.Project.Name,
                ProjectNumber = $"CEC0000{task.Deliverable.Project.ProjectNumber}"

            };
        }

    }
}
