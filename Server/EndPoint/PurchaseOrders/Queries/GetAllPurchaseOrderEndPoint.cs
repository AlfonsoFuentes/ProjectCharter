using Server.Database.Entities.PurchaseOrders;
using Shared.Enums.CurrencyEnums;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.PurchaseOrders.Records;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.Qualitys.Responses;

namespace Server.EndPoint.PurchaseOrders.Queries
{
    public static class GetAllPurchaseOrderEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PurchaseOrders.EndPoint.GetAll, async (PurchaseOrderGetAll Data, IQueryRepository Repository) =>
                {
                    Expression<Func<PurchaseOrder, bool>> Criteria = x => x.PurchaseOrderStatus == Data.Status.Id;
                    if (Data.Status.Id == PurchaseOrderStatusEnum.Approved.Id)
                    {
                        Criteria = x => x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Approved.Id || x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Receiving.Id;

                    }

                    Func<IQueryable<PurchaseOrder>, IIncludableQueryable<PurchaseOrder, object>> Includes = x => x
                    .Include(x => x.Project)
                    .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.BudgetItem!)
                    //.Include(p => p.PurchaseOrderItems).ThenInclude(x => x.BasicEngineeringItem!)
                    .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                    .Include(x => x.Supplier!);
                    var cache = Data.Status.Id == PurchaseOrderStatusEnum.Created.Id ?
                                StaticClass.PurchaseOrders.Cache.GetAllCreated :
                                Data.Status.Id == PurchaseOrderStatusEnum.Approved.Id ? StaticClass.PurchaseOrders.Cache.GetAllApproved :
                                StaticClass.PurchaseOrders.Cache.GetAllClosed;
                    var rows = await Repository.GetAllAsync(cache, Criteria: Criteria, Includes: Includes);
                    if (rows == null)
                    {
                        return Result<PurchaseOrderResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.PurchaseOrders.ClassLegend));
                    }
                    PurchaseOrderResponseList response = new()
                    {
                        Items = rows.Select(x => x.Map()).ToList(),
                    };

                    return Result<PurchaseOrderResponseList>.Success(response);

                });
            }
        }
       

    }
}
