using Server.Database.Entities.ProjectManagements;
using Shared.Models.Backgrounds.Validators;
using Shared.Models.StakeHolders.Validators;

namespace Server.EndPoint.StakeHolders.Vallidators
{
    public static class ValidateStakeHolderNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.StakeHolders.EndPoint.Validate, async (ValidateStakeHolderRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<StakeHolder, bool>> CriteriaId = null!;

                    Func<StakeHolder, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);

                    string CacheKey = StaticClass.StakeHolders.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
