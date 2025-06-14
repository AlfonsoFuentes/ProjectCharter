using Shared.Models.Bennefits.Validators;
using Shared.Models.Backgrounds.Validators;
using Server.Database.Entities.ProjectManagements;

namespace Server.EndPoint.Bennefits.Validators
{
    public static class ValidateBennefitsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Bennefits.EndPoint.Validate, async (ValidateBennefitRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Bennefit, bool>> CriteriaId = x => x.ProjectId == Data.ProjectId;
                    Func<Bennefit, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.Bennefits.Cache.GetAll(Data.ProjectId);

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
