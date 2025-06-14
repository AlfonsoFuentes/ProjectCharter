

using Server.EndPoint.MeetingsGroup.MeetingAgreements.Queries;
using Server.EndPoint.MeetingsGroup.MeetingAttendants.Queries;

namespace Server.EndPoint.MeetingsGroup.Meetings.Queries
{
    public static class GetMeetingByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Meetings.EndPoint.GetById, async (GetMeetingByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Meeting>, IIncludableQueryable<Meeting, object>> Includes = x => x
                    .Where(x => x.Id == request.Id)
                    .Include(x => x.MeetingAttendants).ThenInclude(x => x.StakeHolder!)
                    .Include(x => x.MeetingAgreements)


                    ;

                    Expression<Func<Meeting, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Meetings.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map();
                    return Result.Success(response);

                });
            }
        }
        public static MeetingResponse Map(this Meeting row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                DateofMeeting = row.DateofMeeting,
                MeetingType = row.MeetingType,
                Subject = row.Subject,
                ProjectId = row.ProjectId,
               
                Attendants = row.MeetingAttendants == null || row.MeetingAttendants.Count == 0 ? new() :
                row.MeetingAttendants.Select(x => x.Map(row.ProjectId)).ToList(),

                Agreements = row.MeetingAgreements == null || row.MeetingAgreements.Count == 0 ? new() :
                row.MeetingAgreements.Select(x => x.Map(row.ProjectId)).ToList(),
                Order = row.Order,

            };
        }


    }
}
