using Server.EndPoint.MeetingsGroup.MeetingAttendants.Queries;
using Server.EndPoint.Projects.Queries;
using Server.EndPoint.StakeHolders.Queries;
using Shared.Models.MeetingAttendants.Records;
using Shared.Models.MeetingAttendants.Responses;

namespace Server.EndPoint.MeetingsGroup.MeetingAttendants.Queries
{
    public static class GetMeetingAttendantByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MeetingAttendants.EndPoint.GetById, async (GetMeetingAttendantByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<MeetingAttendant>, IIncludableQueryable<MeetingAttendant, object>> Includes = x =>
                    x.Include(x => x.Meeting)
                    .Include(x => x.StakeHolder!)

                    ;


                    Expression<Func<MeetingAttendant, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.MeetingAttendants.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria, Includes: Includes);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map(row.Meeting.ProjectId);
                    return Result.Success(response);

                });
            }
        }

        public static MeetingAttendantResponse Map(this MeetingAttendant row, Guid ProjectId)
        {
            return new()
            {
                Id = row.Id,
                MeetingId = row.MeetingId,
                ProjectId = ProjectId,
                StakeHolder = row.StakeHolder == null ? null! : row.StakeHolder.Map(),


            };
        }

    }
}
