using Server.Database.Entities.ProjectManagements;
using Shared.Models.MonitoringLogs.Requests;
using Shared.Models.MonitoringLogs.Responses;
using System.Threading;

namespace Server.EndPoint.MonitoringLogs.Commands
{

    public static class CreateUpdateMonitoringLogEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MonitoringLogs.EndPoint.CreateUpdate, async (MonitoringLogResponse Data, IRepository Repository) =>
                {

                    MonitoringLog? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<MonitoringLog, Project>(Data.ProjectId);

                        row = MonitoringLog.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<MonitoringLog>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }

                    Data.Map(row);


                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));


                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
            private string[] GetCacheKeys(MonitoringLog row)
            {
                List<string> cacheKeys = [
                    .. StaticClass.MonitoringLogs.Cache.Key(row.Id, row.ProjectId)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }


        static MonitoringLog Map(this MonitoringLogResponse request, MonitoringLog row)
        {
            row.Name = request.Name;
            row.InitialDate = request.InitialDate;
            row.EndDate = request.EndDate;  
            row.ClosingText = request.ClosingText;
            return row;
        }

    }

}
