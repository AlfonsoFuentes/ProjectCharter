using Shared.Models.Brands.Responses;

namespace Server.EndPoint.Brands.Commands
{

    public static class CreateUpdateBrandEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Brands.EndPoint.CreateUpdate, async (BrandResponse Data, IRepository Repository) =>
                {
                    Brand? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = Brand.Create();

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<Brand>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }
              

                    Data.Map(row);
                    List<string> cache = [.. StaticClass.Brands.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static Brand Map(this BrandResponse request, Brand row)
        {
            row.Name = request.Name;
            return row;
        }

    }

}
