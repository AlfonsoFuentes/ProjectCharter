using Server.Database.Entities.ProjectManagements;
using Shared.Models.Objectives.Validators;

namespace Server.EndPoint.Objectives.Validators
{
    public static class ValidateObjectiveNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Objectives.EndPoint.Validate, async (ValidateObjectiveRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Objective, bool>> CriteriaId = x => x.ProjectId == Data.ProjectId;
                    Func<Objective, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.Objectives.Cache.GetAll(Data.ProjectId);

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
