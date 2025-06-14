using Server.Database.Entities.PurchaseOrders;
using Shared.Models.PurchaseOrders.Requests;

namespace Server.EndPoint.PurchaseOrders.Commands
{
    public static class DeletePurchaseOrderEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PurchaseOrders.EndPoint.Delete, async (DeletePurchaseOrderRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<PurchaseOrder>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }

                    var cachekey = GetCacheKeys(row);

                    await Repository.RemoveAsync(row);
                   
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cachekey);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
            private string[] GetCacheKeys(PurchaseOrder row)
            {
                
                List<string> cacheKeys = [
                 ..StaticClass.PurchaseOrders.Cache.Key(row.Id, row.ProjectId),
               
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
