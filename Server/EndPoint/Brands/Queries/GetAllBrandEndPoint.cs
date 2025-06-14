using Shared.Models.Brands.Records;
using Shared.Models.Brands.Responses;

namespace Server.EndPoint.Brands.Queries
{
    public static class GetAllBrandEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Brands.EndPoint.GetAll, async (BrandGetAll request, IQueryRepository Repository) =>
                {


                    string CacheKey = StaticClass.Brands.Cache.GetAll;
                    var rows = await Repository.GetAllAsync<Brand>(CacheKey);

                    if (rows == null)
                    {
                        return Result<BrandResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Brands.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    BrandResponseList response = new BrandResponseList()
                    {
                        Items = maps
                    };
                    return Result<BrandResponseList>.Success(response);

                });
            }
        }
    }
}