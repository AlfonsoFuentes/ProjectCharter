using Server.Database.Entities.PurchaseOrders;
using Shared.Models.PurchaseOrders.Validators;

namespace Server.EndPoint.PurchaseOrders.Validators
{
    public static class ValidatePurchaseOrdersNumberEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PurchaseOrders.EndPoint.ValidateNumber, async (ValidatePurchaseOrderNumberRequest Data, IQueryRepository Repository) =>
                {
                 
                    Func<PurchaseOrder, bool> CriteriaExist = x => Data.Id == null ?
                    x.PONumber.Equals(Data.Number) : x.Id != Data.Id.Value && x.PONumber.Equals(Data.Number);
                    string CacheKey = StaticClass.PurchaseOrders.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist);
                });


            }
        }



    }
}
