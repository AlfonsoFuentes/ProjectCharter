using Server.Database.Entities.ProjectManagements;
using Shared.Models.MonitoringLogs.Mappers;

namespace Server.EndPoint.MonitoringLogs.Commands
{
    public static class ChangeMonitoringLogOrderUpEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MonitoringLogs.EndPoint.UpdateUp, async (ChangeMonitoringLogOrderUpRequest Data, IRepository Repository) =>
                {


                    var row = await Repository.GetByIdAsync<MonitoringLog>(Data.Id);

                    if (row == null) { return Result.Fail(Data.NotFound); }
                    if (row.Order == 1) { return Result.Success(Data.Succesfully); }

                    Expression<Func<MonitoringLog, bool>> Criteria = x => x.ProjectId == Data.ProjectId && x.Order == row.Order - 1;

                    var previousRow = await Repository.GetAsync(Criteria: Criteria);

                    if (previousRow == null) { return Result.Fail(Data.NotFound); }

                    await Repository.UpdateAsync(previousRow);
                    await Repository.UpdateAsync(row);

                    row.Order = row.Order - 1;
                    previousRow.Order = row.Order + 1;

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



    }

}
