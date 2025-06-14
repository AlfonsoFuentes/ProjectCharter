using Shared.Models.Meetings.Requests;

namespace Server.EndPoint.MeetingsGroup.Meetings.Commands
{
    public static class DeleteMeetingEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Meetings.EndPoint.Delete, async (DeleteMeetingRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<Meeting>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    var cache = GetCacheKeys(row);

                    await Repository.RemoveAsync(row);

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
            private string[] GetCacheKeys(Meeting row)
            {
                List<string> cacheKeys = [
                    StaticClass.Meetings.Cache.GetAll
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }
    }
}
