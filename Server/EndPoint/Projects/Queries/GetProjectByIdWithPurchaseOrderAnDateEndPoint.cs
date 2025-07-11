using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Enums.CostCenter;
using Shared.Enums.Focuses;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.PurchaseOrders.Responses;
using System.Collections.Generic;

namespace Server.EndPoint.Projects.Queries
{
    public static class GetProjectByIdWithPurchaseOrderAnDateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Projects.EndPoint.GetByIdWithPurchaseOrderAndDate, async (GetProjectWithPurchaseOrderGetByIdAndDate request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> Includes = x => x
                      .Include(x => x.PurchaseOrders).ThenInclude(po => po.PurchaseOrderItems).ThenInclude(poi => poi.PurchaseOrderReceiveds)
                     ;

                    Expression<Func<Project, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Projects.Cache.GetPurchaseOrderById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria, Includes: Includes);

                    if (row == null)
                    {
                        return Result<ProjectWithPurchaseOrdersResponse>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Projects.ClassLegend));
                    }

                    ProjectWithPurchaseOrdersResponse response = new()
                    {
                        Id = request.Id,
                        Name = row.Name,


                    };
                    
                    var purchaseOrders = GetCombinedPurchaseOrders(row, request.Year, request.Month);
                    response.PurchaseOrders.AddRange(purchaseOrders);
                    return Result<ProjectWithPurchaseOrdersResponse>.Success(response);

                });
            }
            public List<PurchaseOrderResponse> GetCombinedPurchaseOrders(Project row, int year, int month)
            {
                var received = GetReceivedOrders(row, year, month);
                var expected = GetExpectedOrders(row, year, month);

                var comparer = new PurchaseOrderResponseComparer();

                return received.Union(expected, comparer).OrderBy(x => x.PONumber).ToList();
            }
            private List<PurchaseOrderResponse> GetReceivedOrders(Project row, int year, int month)
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
                        if (queryPurchaseOrderReceiveds.Any())
                        {
                            result.Add(purchaseorderMap);
                            foreach (var purchaseOrderReceived in queryPurchaseOrderReceiveds)
                            {
                                var purchaseOrderReceivedMap = purchaseOrderReceived.Map();
                                purchaseOrderItemMap.PurchaseOrderItemReceiveds.Add(purchaseOrderReceivedMap);

                            }
                        }
                        
                    }
                }

                return result;
            }
            
            private List<PurchaseOrderResponse> GetExpectedOrders(Project row, int year, int month)
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
    public class PurchaseOrderResponseComparer : IEqualityComparer<PurchaseOrderResponse>
    {
        public bool Equals(PurchaseOrderResponse? x, PurchaseOrderResponse? y)
        {
            if (x == null || y == null) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(PurchaseOrderResponse obj)
        {
            return obj?.Id.GetHashCode() ?? 0;
        }
    }
}
