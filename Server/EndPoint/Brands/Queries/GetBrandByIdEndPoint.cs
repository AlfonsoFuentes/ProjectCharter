using Shared.Models.Brands.Records;
using Shared.Models.Brands.Responses;

namespace Server.EndPoint.Brands.Queries
{
    public static class GetBrandByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Brands.EndPoint.GetById, async (GetBrandByIdRequest request, IQueryRepository Repository) =>
                {
                   
                    Expression<Func<Brand, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Brands.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map();
                    return Result.Success(response);

                });
            }
        }

        public static BrandResponse Map(this Brand row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
               
            };
        }

    }
}
