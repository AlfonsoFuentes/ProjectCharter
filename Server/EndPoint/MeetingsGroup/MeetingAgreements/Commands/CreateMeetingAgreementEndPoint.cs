using Shared.Models.MeetingAgreements.Requests;

namespace Server.EndPoint.MeetingsGroup.MeetingAgreements.Commands
{
    public static class CreateMeetingAgreementEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MeetingAgreements.EndPoint.Create, async (CreateMeetingAgreementRequest Data, IRepository Repository) =>
                {
                    var row = MeetingAgreement.Create(Data.MeetingId);

                    await Repository.AddAsync(row);

                    Data.Map(row);
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row,Data.ProjectId));

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });
            }
            private string[] GetCacheKeys(MeetingAgreement row, Guid ProjectId)
            {
                List<string> cacheKeys = [
                 
                    .. StaticClass.Meetings.Cache.Key(row.MeetingId),
                    .. StaticClass.MeetingAgreements.Cache.Key(row.Id)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }


        static MeetingAgreement Map(this CreateMeetingAgreementRequest request, MeetingAgreement row)
        {
            row.Name = request.Name;


            return row;
        }

    }
}
