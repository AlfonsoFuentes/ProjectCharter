using Server.Database.Entities.PurchaseOrders;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Enums.TaskStatus;
using Shared.Models.Suppliers.Records;
using Shared.Models.Suppliers.Responses;
using Shared.Models.Warnings.Records;
using Shared.Models.Warnings.Responses;

namespace Server.EndPoint.Suppliers.Queries
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

                    response.Items.AddRange(await WarningFromPurchaseOrders(request, Repository));
                    response.Items.AddRange(await WarningFromGanttTaskDelayedTask(request, Repository));
                    response.Items.AddRange(await WarningFromGanttTaskNotClosed(request, Repository));

                    return Result.Success(response);

                });
            }
            async Task<List<WarningResponse>> WarningFromPurchaseOrders(WarningGetAll request, IQueryRepository Repository)
            {
                DateTime now = DateTime.Now;
                Expression<Func<PurchaseOrder, bool>> Criteria = x => x.ExpectedDate <= now &&
                    (x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Approved.Id || x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Receiving.Id);

                Func<IQueryable<PurchaseOrder>, IIncludableQueryable<PurchaseOrder, object>> Includes = x => x
                .Include(x => x.Project)
                .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.BudgetItem!)
                .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.BasicEngineeringItem!)
                .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                .Include(x => x.Supplier!);
                var cache = StaticClass.PurchaseOrders.Cache.GetAllApproved;
                var PurchaseOrderRows = await Repository.GetAllAsync(cache, Criteria: Criteria, Includes: Includes);
                return PurchaseOrderRows.Select(x => x.MapWarning()).ToList();
            }
            async Task<List<WarningResponse>> WarningFromGanttTaskDelayedTask(WarningGetAll request, IQueryRepository Repository)
            {
                DateTime now = DateTime.Now;
                var cache = StaticClass.DeliverableGanttTasks.Cache.GetAllProjects;
                Func<IQueryable<NewGanttTask>, IIncludableQueryable<NewGanttTask, object>> includes = x => x
                  .Include(x => x.Deliverable)
               ;
                Expression<Func<NewGanttTask, bool>> criteria = x =>  x.TaskStatus == GanttTaskStatusEnum.NotInitiated.Id && x.StartDate < now;
                var gantTasks = await Repository.GetAllAsync(Cache: cache, Criteria: criteria, Includes: includes);

                return gantTasks.Select(x => x.MapWarningDelayed()).ToList();
            }
            async Task<List<WarningResponse>> WarningFromGanttTaskNotClosed(WarningGetAll request, IQueryRepository Repository)
            {
                DateTime now = DateTime.Now;
                var cache = StaticClass.DeliverableGanttTasks.Cache.GetAllProjects;
                Func<IQueryable<NewGanttTask>, IIncludableQueryable<NewGanttTask, object>> includes = x => x
                  .Include(x => x.Deliverable)
               ;
                Expression<Func<NewGanttTask, bool>> criteria = x =>  x.TaskStatus == GanttTaskStatusEnum.OnGoing.Id && x.EndDate > now;
                var gantTasks = await Repository.GetAllAsync(Cache: cache, Criteria: criteria, Includes: includes);

                return gantTasks.Select(x => x.MapWarningNotClosed()).ToList();
            }
        }
    }
}