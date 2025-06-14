using Server.Database.Entities.ProjectManagements;
using Shared.Models.MonitoringLogs.Requests;

namespace Server.EndPoint.MonitoringLogs.Commands
{
    public static class DeleteMonitoringLogEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MonitoringLogs.EndPoint.Delete, async (DeleteMonitoringLogRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<MonitoringLog>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    var cache = GetCacheKeys(row);

                    await Repository.RemoveAsync(row);
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);

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




    }

}
