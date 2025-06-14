using Server.Database.Entities.PurchaseOrders;
using Shared.Models.Suppliers.Records;
using Shared.Models.Suppliers.Responses;

namespace Server.EndPoint.Suppliers.Queries
{
    public static class GetAllSupplierEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Suppliers.EndPoint.GetAll, async (SupplierGetAll request, IQueryRepository Repository) =>
                {


                    string CacheKey = StaticClass.Suppliers.Cache.GetAll;
                    var rows = await Repository.GetAllAsync<Supplier>(CacheKey);

                    if (rows == null)
                    {
                        return Result<SupplierResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Suppliers.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    SupplierResponseList response = new SupplierResponseList()
                    {
                        Items = maps
                    };
                    return Result<SupplierResponseList>.Success(response);

                });
            }
        }
    }
}