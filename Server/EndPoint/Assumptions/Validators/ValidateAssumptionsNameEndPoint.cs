using Server.Database.Entities.ProjectManagements;
using Shared.Models.Assumptions.Validators;
using Shared.Models.Backgrounds.Validators;

namespace Server.EndPoint.Assumptions.Validators
{
    public static class ValidateAssumptionsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Assumptions.EndPoint.Validate, async (ValidateAssumptionRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Assumption, bool>> CriteriaId = x => x.ProjectId == Data.ProjectId;
                    Func<Assumption, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.Assumptions.Cache.GetAll(Data.ProjectId);

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
