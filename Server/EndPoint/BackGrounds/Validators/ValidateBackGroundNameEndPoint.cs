using Server.Database.Entities.ProjectManagements;
using Shared.Models.Backgrounds.Validators;

namespace Server.EndPoint.BackGrounds.Validators
{
    public static class ValidateBackGroundNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BackGrounds.EndPoint.Validate, async (ValidateBackGroundRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<BackGround, bool>> CriteriaId = x => x.ProjectId == Data.ProjectId;
                    Func<BackGround, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.BackGrounds.Cache.GetAll(Data.ProjectId);

                    var result= await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                    return result;
                });


            }
        }



    }

}
