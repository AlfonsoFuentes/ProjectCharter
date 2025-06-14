using Shared.Models.ExpendingTools.Records;
using Shared.Models.ExpendingTools.Responses;
using Shared.Models.MonitoringExpendingTools.Responses;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace Server.EndPoint.MonitoringExpendingTools.Queries
{
    public static class GetAllMonitoringExpendingToolEndPoint
    {
        private static readonly CultureInfo _CurrentCulture = new("en-US");

        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MonitoringExpendingTools.EndPoint.GetAll,
                    async (GetAllMonitoringExpendingTool request, IQueryRepository repository) =>
                    {
                        string cacheKey = StaticClass.BudgetItems.Cache.GetAllWithPurchaseOrder(request.ProjectId);
                        Func<IQueryable<BudgetItem>, IIncludableQueryable<BudgetItem, object>> includes = x => x
                           .Include(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                           .Include(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrder)
                           .Include(x => x.PurchaseOrderItems).ThenInclude(x => x.BasicEngineeringItem)
                           .Include(x => (x as Instrument)!.InstrumentItems!).ThenInclude(x => x.InstrumentTemplate!)
                           .Include(x => (x as Pipe)!.PipeItems!).ThenInclude(x => x.PipeTemplate)
                           .Include(x => (x as Valve)!.ValveItems!).ThenInclude(x => x.ValveTemplate!)
                           .Include(x => (x as Equipment)!.EquipmentItems!).ThenInclude(x => x.EquipmentTemplate!)
                           .Include(x => x.BudgetItemNewGanttTasks).ThenInclude(x => x.NewGanttTask).ThenInclude(x => x.MainTasks);

                        Expression<Func<BudgetItem, bool>> criteria = x => x.ProjectId == request.ProjectId;
                        var budgetitems = await repository.GetAllAsync(Cache: cacheKey, Criteria: criteria, Includes: includes);

                        if (budgetitems is null || !budgetitems.Any())
                            return Result<BudgetItemMonitoringReportDtoList>.Fail(request.NotFound);

                        var service = new BudgetReportService(_CurrentCulture);
                        var result = new BudgetItemMonitoringReportDtoList
                        {
                            Items = service.Generate(budgetitems, DateTime.Now)
                        };

                        return Result<BudgetItemMonitoringReportDtoList>.Success(result);
                    });
            }
        }
    }
}