using Server.Database.Entities.PurchaseOrders;
using Shared.Models.PurchaseOrders.Validators;

namespace Server.EndPoint.PurchaseOrders.Validators
{
    public static class ValidatePurchaseOrdersRequisitionEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PurchaseOrders.EndPoint.ValidatePR, async (ValidatePurchaseOrderRequisitionRequest Data, IQueryRepository Repository) =>
                {
                    //Expression<Func<PurchaseOrder, bool>> CriteriaId = null!;
                    Func<PurchaseOrder, bool> CriteriaExist = x => Data.Id == null ?
                    x.PurchaseRequisition.Equals(Data.PurchaseRequisition) : x.Id != Data.Id.Value && 
                    x.PurchaseRequisition.Equals(Data.PurchaseRequisition);
                    string CacheKey = StaticClass.PurchaseOrders.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist);
                });


            }
        }



    }
}
