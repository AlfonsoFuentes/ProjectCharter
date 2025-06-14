using Server.Database.Entities.BudgetItems.Commons;
using Server.EndPoint.MonitoringTasks.Queries;
using Server.EndPoint.Projects.Queries;
using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Models.BudgetItems.IndividualItems.Taxs.Records;
using Shared.Models.BudgetItems.IndividualItems.Taxs.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.Taxs.Queries
{
    public static class GetTaxByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Taxs.EndPoint.GetById, async (GetTaxByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Tax>, IIncludableQueryable<Tax, object>> Includes = x => x
                    .Include(x => x.TaxesItems!).ThenInclude(x => x.Selected!)
                    ;


                    Expression<Func<Tax, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Taxs.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria, Includes: Includes);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map();
                    return Result.Success(response);

                });
            }
        }
        public static TaxResponse Map(this Tax row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                OrderList = row.OrderList,
                ProjectId = row.ProjectId,
                Nomenclatore = row.Nomenclatore,
                BudgetUSD = row.BudgetUSD,
                Percentage = row.Percentage,
                TaxItems = row.TaxesItems == null || row.TaxesItems.Count == 0 ? new() : row.TaxesItems.Select(x => x.Map()).ToList(),
                ActualUSD = row.ActualUSD,
                CommitmentUSD = row.CommitmentUSD,
                PotentialUSD = row.PotentialUSD,
                PurchaseOrders = row.PurchaseOrderItems == null ? new() : row.PurchaseOrderItems.Select(x => x.PurchaseOrder).Select(x => x.Map()).ToList(),
                BudgetItemGanttTasks = row.BudgetItemNewGanttTasks == null ? new() : row.BudgetItemNewGanttTasks.Select(x => x.Map()).ToList(),
            };
        }
        public static TaxItemResponse Map(this TaxesItem row)
        {
            return new()
            {
                Id = row.Id,
                Budget = row.Selected == null ? 0 : row.Selected.BudgetUSD,
                BudgetItemId = row.SelectedId,
                Name = row.Selected == null ? string.Empty : row.Selected.Name,
                Nomenclatore = row.Selected == null ? string.Empty : row.Selected.Nomenclatore,

            };
        }

    }
}
