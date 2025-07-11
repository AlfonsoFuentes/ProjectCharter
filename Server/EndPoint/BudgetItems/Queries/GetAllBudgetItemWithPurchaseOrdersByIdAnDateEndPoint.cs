using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.PurchaseOrders.Responses;

namespace Server.EndPoint.BudgetItems.Queries
{
    public static class GetAllBudgetItemWithPurchaseOrdersByIdAnDateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BudgetItems.EndPoint.GetWithPurchaseorderByIdAndDate, async (BudgetItemWithPurchaseOrderGetByIdAndDate request, IQueryRepository repository) =>
                {
                    Func<IQueryable<BudgetItem>, IIncludableQueryable<BudgetItem, object>> includes = x => x
                    .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrder).ThenInclude(x => x.Supplier)
                    .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                    ;

                    Expression<Func<BudgetItem, bool>> criteria = x => x.Id == request.Id;
                    string cacheKey = StaticClass.BudgetItems.Cache.GetAllWithPurchaseOrder(request.Id, request.ProjectId);

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



                    };




                    var purchaseOrders = GetCombinedPurchaseOrders(row, request.Year, request.Month);
                    response.PurchaseOrders.AddRange(purchaseOrders);
                    return Result<BudgetItemWithPurchaseOrdersResponse>.Success(response);
                });
            }
            public List<PurchaseOrderResponse> GetCombinedPurchaseOrders(BudgetItem row, int year, int month)
            {
                var received = GetReceivedOrders(row, year, month);
                var expected = GetExpectedOrders(row, year, month);

                var comparer = new PurchaseOrderResponseComparer();

                return received.Union(expected, comparer).OrderBy(x => x.PONumber).ToList();
            }
           
            private List<PurchaseOrderResponse> GetReceivedOrders(BudgetItem row, int year, int month)
            {
                List<PurchaseOrderResponse> result = new();

                var queryPurchaseOrders = row.PurchaseOrders
                    .Where(x => x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Closed.Id || x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Receiving.Id).ToList();
                foreach (var purchaseOrder in queryPurchaseOrders)
                {
                    var purchaseorderMap = purchaseOrder.SingleMap();
                  
                    foreach (var purchaseOrderItem in purchaseOrder.PurchaseOrderItems)
                    {
                        var purchaseOrderItemMap = purchaseOrderItem.SingleMap();

                        purchaseorderMap.PurchaseOrderItems.Add(purchaseOrderItemMap);

                        var queryPurchaseOrderReceiveds = purchaseOrderItem.PurchaseOrderReceiveds
                            .Where(por => por.CurrencyDate.HasValue && por.CurrencyDate.Value.Year == year);

                        if (month != -1)
                        {
                            queryPurchaseOrderReceiveds = queryPurchaseOrderReceiveds
                                .Where(por => por.CurrencyDate.HasValue && por.CurrencyDate.Value.Year == year && por.CurrencyDate!.Value.Month == month).ToList();
                        }
                        if(queryPurchaseOrderReceiveds.Any())
                        {
                            result.Add(purchaseorderMap);
                        }
                        foreach (var purchaseOrderReceived in queryPurchaseOrderReceiveds)
                        {
                            var purchaseOrderReceivedMap = purchaseOrderReceived.Map();
                            purchaseOrderItemMap.PurchaseOrderItemReceiveds.Add(purchaseOrderReceivedMap);
                           
                        }
                    }
                }

                return result;
            }
            private List<PurchaseOrderResponse> GetExpectedOrders(BudgetItem row, int year, int month)
            {
                DateTime QueryDate = DateTime.Now;
                if (year == QueryDate.Year && month < QueryDate.Month)
                {
                    return new List<PurchaseOrderResponse>();
                }
                var query = row.PurchaseOrders.Where(x => x.PurchaseOrderStatus != PurchaseOrderStatusEnum.Closed.Id)
                    .SelectMany(po => po.PurchaseOrderItems)
                    .Where(poi => poi.PurchaseOrder != null &&
                                  poi.PurchaseOrder.ExpectedDate.HasValue &&
                                  poi.PurchaseOrder.ExpectedDate.Value.Year == year);

                if (month != -1)
                {
                    query = query.Where(poi => poi.PurchaseOrder.ExpectedDate!.Value.Month == month);
                }

                return query
                    .Select(x => x.PurchaseOrder.Map())
                    .ToList();
            }


        }
    }
}