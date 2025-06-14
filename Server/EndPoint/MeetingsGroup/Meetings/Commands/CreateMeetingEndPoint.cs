using Shared.Models.Meetings.Requests;

namespace Server.EndPoint.MeetingsGroup.Meetings.Commands
{

    public static class CreateMeetingEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Meetings.EndPoint.Create, async (CreateMeetingRequest Data, IRepository Repository) =>
                {
                    var lastorder = await Repository.GetLastOrderAsync<Meeting, Project>(Data.ProjectId);
                    var row = Meeting.Create(Data.ProjectId, lastorder);

                    await Repository.AddAsync(row);

                    Data.Map(row);
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

              
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);
                });


            }


            private string[] GetCacheKeys(Meeting row)
            {
                List<string> cacheKeys = [
               
                    .. StaticClass.Meetings.Cache.Key(row.Id)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }


        static Meeting Map(this CreateMeetingRequest request, Meeting row)
        {
            row.Name = request.Name;
            row.DateofMeeting = request.DateofMeeting;
            row.Subject = request.Subject;
            row.MeetingType = request.MeetingType.Name;
            return row;
        }

    }
}
