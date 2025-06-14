using Shared.Models.MeetingAgreements.Validators;
using Shared.Models.Backgrounds.Validators;

namespace Server.EndPoint.MeetingsGroup.MeetingAgreements.Validators
{
    public static class ValidateMeetingAgreementsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MeetingAgreements.EndPoint.Validate, async (ValidateMeetingAgreementRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<MeetingAgreement, bool>> CriteriaId = x => x.MeetingId == Data.MeetingId;

                    Func<MeetingAgreement, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);

                    string CacheKey = StaticClass.MeetingAgreements.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
