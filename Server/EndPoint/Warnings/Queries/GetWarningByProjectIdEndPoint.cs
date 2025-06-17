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
                    response.Items.AddRange(await WarningFromGanttTaskDelayedTask(request, Repository));
                    response.Items.AddRange(await WarningFromGanttTaskNotClosed(request, Repository));

                    return Result.Success(response);

                });
            }
            async Task<List<WarningResponse>> WarningFromPurchaseOrders(WarningGetByProjectId request, IQueryRepository Repository)
            {
                DateTime now = DateTime.Now;
                Expression<Func<PurchaseOrder, bool>> Criteria = x => x.ExpectedDate <= now && x.ProjectId == request.ProjectId &&
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
            async Task<List<WarningResponse>> WarningFromGanttTaskDelayedTask(WarningGetByProjectId request, IQueryRepository Repository)
            {
                DateTime now = DateTime.Now;
                var cache = StaticClass.DeliverableGanttTasks.Cache.GetAll(request.ProjectId);
                Func<IQueryable<NewGanttTask>, IIncludableQueryable<NewGanttTask, object>> includes = x => x
                  .Include(x => x.Deliverable)
               ;
                Expression<Func<NewGanttTask, bool>> criteria = x => x.ProjectId == request.ProjectId && x.TaskStatus == GanttTaskStatusEnum.NotInitiated.Id && x.StartDate < now;
                var gantTasks = await Repository.GetAllAsync(Cache: cache, Criteria: criteria, Includes: includes);

                return gantTasks.Select(x => x.MapWarningDelayed()).ToList();
            }
            async Task<List<WarningResponse>> WarningFromGanttTaskNotClosed(WarningGetByProjectId request, IQueryRepository Repository)
            {
                DateTime now = DateTime.Now;
                var cache = StaticClass.DeliverableGanttTasks.Cache.GetAll(request.ProjectId);
                Func<IQueryable<NewGanttTask>, IIncludableQueryable<NewGanttTask, object>> includes = x => x
                  .Include(x => x.Deliverable)
               ;
                Expression<Func<NewGanttTask, bool>> criteria = x => x.ProjectId == request.ProjectId && x.TaskStatus == GanttTaskStatusEnum.OnGoing.Id && x.EndDate > now;
                var gantTasks = await Repository.GetAllAsync(Cache: cache, Criteria: criteria, Includes: includes);

                return gantTasks.Select(x => x.MapWarningNotClosed()).ToList();
            }
        }
       public static WarningResponse MapWarning(this PurchaseOrder purchaseOrder)
        {
            return new WarningResponse
            {
                PurchaseOrder = purchaseOrder.Map(),
                WarningText = $"Purchase Order {purchaseOrder.PONumber} is overdue for delivery. Expected date was {purchaseOrder.ExpectedDate!.Value.ToShortDateString()}."
            };
        }
        public static WarningResponse MapWarningDelayed(this NewGanttTask task)
        {
            return new WarningResponse
            {
                GanttTask = task.MapMonitoringParallel(),
                WarningText = $"Task {task.Name} is delayed. It was supposed to start on {task.StartDate.ToShortDateString()}."
            };
        }
        public static WarningResponse MapWarningNotClosed(this NewGanttTask task)
        {
            return new WarningResponse
            {
                GanttTask = task.MapMonitoringParallel(),
                WarningText = $"Task {task.Name} is not closed. It was supposed to end on {task.EndDate.ToShortDateString()}."

            };
        }

    }
}
