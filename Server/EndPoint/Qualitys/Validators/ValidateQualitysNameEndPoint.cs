using Shared.Models.Qualitys.Validators;
using Shared.Models.Backgrounds.Validators;
using Server.Database.Entities.ProjectManagements;

namespace Server.EndPoint.Qualitys.Validators
{
    public static class ValidateQualitysNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Qualitys.EndPoint.Validate, async (ValidateQualityRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Quality, bool>> CriteriaId = x => x.ProjectId == Data.ProjectId;
                    Func<Quality, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.Qualitys.Cache.GetAll(Data.ProjectId);

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
