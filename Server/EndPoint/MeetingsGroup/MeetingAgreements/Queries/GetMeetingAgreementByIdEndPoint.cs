using Server.EndPoint.Projects.Queries;
using Shared.Models.MeetingAgreements.Records;
using Shared.Models.MeetingAgreements.Responses;

namespace Server.EndPoint.MeetingsGroup.MeetingAgreements.Queries
{
    public static class GetMeetingAgreementByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MeetingAgreements.EndPoint.GetById, async (GetMeetingAgreementByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<MeetingAgreement>, IIncludableQueryable<MeetingAgreement, object>> Includes = x =>
                    x.Include(x => x.Meeting);

                    ;

                    Expression<Func<MeetingAgreement, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.MeetingAgreements.Cache.GetById(request.Id);
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

        public static MeetingAgreementResponse Map(this MeetingAgreement row, Guid ProjectId)
        {
            return new()
            {
                Id = row.Id,

                MeetingId = row.MeetingId,
                ProjectId = ProjectId,
                Name = row.Name,



            };
        }

    }
}
