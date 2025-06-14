using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;

namespace Server.EndPoint.BudgetItems.Queries
{
    public static class GetAllBudgetItemWithPurchaseOrdersByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BudgetItems.EndPoint.GetWithPurchaseorderById, async (BudgetItemWithPurchaseOrderGetById request, IQueryRepository repository) =>
                {
                    Func<IQueryable<BudgetItem>, IIncludableQueryable<BudgetItem, object>> includes = x => x
                    .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrder)
                    .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                    ;

                    Expression<Func<BudgetItem, bool>> criteria = x => x.Id == request.Id;
                    string cacheKey = StaticClass.BudgetItems.Cache.GetByIdWithPurchaseOrder(request.Id);

                    var row = await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);

                    if (row == null)
                    {
                        return Result<BudgetItemWithPurchaseOrdersResponse>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.BudgetItems.ClassLegend));
                    }

                    BudgetItemWithPurchaseOrdersResponse response = new()
                    {
                        Id = row.Id,
                       
                       
                        Name = row.Name,
                        Nomenclatore = row.Nomenclatore,
            
                        ProjectId = row.ProjectId,
                        IsAlteration = row.IsAlteration,
                        IsTaxes = row.IsTaxes,

                        BudgetUSD = row.BudgetUSD,
                        ActualUSD = row.ActualUSD,
                        CommitmentUSD = row.CommitmentUSD,
                        PotentialUSD = row.PotentialUSD,
                        PurchaseOrders=row.PurchaseOrderItems.Select(x=>x.PurchaseOrder.Map()).ToList(),


                    };



                    return Result<BudgetItemWithPurchaseOrdersResponse>.Success(response);
                });
            }



        }
    }
}