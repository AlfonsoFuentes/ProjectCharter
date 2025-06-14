using Server.Database.Entities.PurchaseOrders;
using Shared.Models.Suppliers.Requests;

namespace Server.EndPoint.Suppliers.Commands
{
    public static class DeleteSupplierEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Suppliers.EndPoint.Delete, async (DeleteSupplierRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<Supplier>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.Suppliers.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }

    }
}
