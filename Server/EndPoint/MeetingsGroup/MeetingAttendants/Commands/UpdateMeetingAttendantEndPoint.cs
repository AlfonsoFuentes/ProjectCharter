using Shared.Models.MeetingAttendants.Requests;


namespace Server.EndPoint.MeetingsGroup.MeetingAttendants.Commands
{
    public static class UpdateMeetingAttendantEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MeetingAttendants.EndPoint.Update, async (UpdateMeetingAttendantRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<MeetingAttendant>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.UpdateAsync(row);
                    Data.Map(row);
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row, Data.ProjectId));


                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);
                });


            }


            private string[] GetCacheKeys(MeetingAttendant row, Guid ProjectId)
            {
                List<string> cacheKeys = [
                   
                    .. StaticClass.Meetings.Cache.Key(row.MeetingId),
                    .. StaticClass.MeetingAttendants.Cache.Key(row.Id)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }


        static MeetingAttendant Map(this UpdateMeetingAttendantRequest request, MeetingAttendant row)
        {
            row.StakeHolderId = request.StakeHolder.Id;
            return row;
        }

    }
}
