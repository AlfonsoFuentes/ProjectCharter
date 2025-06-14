using Shared.Models.MeetingAttendants.Requests;

namespace Server.EndPoint.MeetingsGroup.MeetingAttendants.Commands
{
    public static class DeleteMeetingAttendantEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MeetingAttendants.EndPoint.Delete, async (DeleteMeetingAttendantRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<MeetingAttendant>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }


                    var cache = GetCacheKeys(row, Data.ProjectId);

                    await Repository.RemoveAsync(row);

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
            private string[] GetCacheKeys(MeetingAttendant row, Guid ProjectId)
            {
                List<string> cacheKeys = [
                 
                    .. StaticClass.Meetings.Cache.Key(row.MeetingId)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }
    }
}
