using Server.Database.Entities.ProjectManagements;
using Shared.Models.AcceptanceCriterias.Validators;
using Shared.Models.Backgrounds.Validators;

namespace Server.EndPoint.AcceptanceCriterias.Validators
{
    public static class ValidateAcceptanceCriteriasNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.AcceptanceCriterias.EndPoint.Validate, async (ValidateAcceptanceCriteriaRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<AcceptanceCriteria, bool>> CriteriaId = x => x.ProjectId == Data.ProjectId;
                    Func<AcceptanceCriteria, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.AcceptanceCriterias.Cache.GetAll(Data.ProjectId);

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
