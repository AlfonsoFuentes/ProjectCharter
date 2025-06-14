using Server.Database.Entities.PurchaseOrders;
using Shared.Models.PurchaseOrders.Validators;

namespace Server.EndPoint.PurchaseOrders.Validators
{
    public static class ValidatePurchaseOrdersNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PurchaseOrders.EndPoint.ValidateName, async (ValidatePurchaseOrderNameRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<PurchaseOrder, bool>> CriteriaId = x => x.Id == Data.ProjectId;
                    Func<PurchaseOrder, bool> CriteriaExist = x => Data.Id == null ?
                    x.PurchaseorderName.Equals(Data.Name) : x.Id != Data.Id.Value && x.PurchaseorderName.Equals(Data.Name);
                    string CacheKey = StaticClass.PurchaseOrders.Cache.GetAllNames(Data.ProjectId);

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }
}
