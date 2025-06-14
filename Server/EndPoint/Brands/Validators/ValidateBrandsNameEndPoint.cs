using Shared.Models.Brands.Validators;
using Shared.Models.Backgrounds.Validators;

namespace Server.EndPoint.Brands.Validators
{
    public static class ValidateBrandsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Brands.EndPoint.Validate, async (ValidateBrandRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Brand, bool>> CriteriaId = null!;
                    Func<Brand, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.Brands.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
