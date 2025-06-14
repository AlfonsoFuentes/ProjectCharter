using Shared.Models.MeetingAttendants.Validators;
using Shared.Models.Backgrounds.Validators;

namespace Server.EndPoint.MeetingsGroup.MeetingAttendants.Validators
{
    public static class ValidateMeetingAttendantsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MeetingAttendants.EndPoint.Validate, async (ValidateMeetingAttendantRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<MeetingAttendant, bool>> CriteriaId = x => x.MeetingId == Data.MeetingId;

                    Func<MeetingAttendant, bool> CriteriaExist = x => Data.Id == null ?
                    x.StakeHolderId == Data.StakeHolderId : x.Id != Data.Id.Value && x.StakeHolderId == Data.StakeHolderId;

                    string CacheKey = StaticClass.MeetingAttendants.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
