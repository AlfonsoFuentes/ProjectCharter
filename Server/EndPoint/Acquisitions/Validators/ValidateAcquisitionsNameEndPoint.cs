using Server.Database.Entities.ProjectManagements;
using Shared.Models.Acquisitions.Validators;
using Shared.Models.Backgrounds.Validators;

namespace Server.EndPoint.Acquisitions.Validators
{
    public static class ValidateAcquisitionsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Acquisitions.EndPoint.Validate, async (ValidateAcquisitionRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Acquisition, bool>> CriteriaId = x => x.ProjectId == Data.ProjectId;
                    Func<Acquisition, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.Acquisitions.Cache.GetAll(Data.ProjectId);

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
