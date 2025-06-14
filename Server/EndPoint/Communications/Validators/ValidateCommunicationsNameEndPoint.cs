using Shared.Models.Communications.Validators;
using Shared.Models.Backgrounds.Validators;
using Server.Database.Entities.ProjectManagements;

namespace Server.EndPoint.Communications.Validators
{
    public static class ValidateCommunicationsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Communications.EndPoint.Validate, async (ValidateCommunicationRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Communication, bool>> CriteriaId = x => x.ProjectId == Data.ProjectId;
                    Func<Communication, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.Communications.Cache.GetAll(Data.ProjectId);

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
