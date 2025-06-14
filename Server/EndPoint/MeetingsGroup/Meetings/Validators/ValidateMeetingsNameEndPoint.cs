using Shared.Models.Meetings.Validators;
using Shared.Models.Backgrounds.Validators;

namespace Server.EndPoint.MeetingsGroup.Meetings.Validators
{
    public static class ValidateMeetingsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Meetings.EndPoint.Validate, async (ValidateMeetingRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Meeting, bool>> CriteriaId = x => x.ProjectId == Data.ProjectId;
                    Func<Meeting, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.Meetings.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
