namespace Server.EndPoint.MeetingsGroup.Meetings.Queries
{
    public static class GetAllMeetingEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Meetings.EndPoint.GetAll, async (MeetingGetAll request, IQueryRepository Repository) =>
                {

                    Expression<Func<Meeting, bool>> Criteria = x => x.ProjectId == request.ProjectId;
                    string CacheKey = StaticClass.Meetings.Cache.GetAll;
                    var rows = await Repository.GetAllAsync(Cache: CacheKey, Criteria: Criteria, OrderBy: x => x.Order);

                    if (rows == null)
                    {
                        return Result<MeetingResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Meetings.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    MeetingResponseList response = new MeetingResponseList()
                    {
                        Items = maps
                    };
                    return Result<MeetingResponseList>.Success(response);

                });
            }
        }
    }
}