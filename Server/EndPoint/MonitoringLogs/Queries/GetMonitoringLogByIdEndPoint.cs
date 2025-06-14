using Server.Database.Entities.ProjectManagements;
using Shared.Models.MonitoringLogs.Records;
using Shared.Models.MonitoringLogs.Responses;

namespace Server.EndPoint.MonitoringLogs.Queries
{
    public static class GetMonitoringLogByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MonitoringLogs.EndPoint.GetById, async (GetMonitoringLogByIdRequest request, IQueryRepository Repository) =>
                {


                    Expression<Func<MonitoringLog, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.MonitoringLogs.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map();
                    return Result.Success(response);

                });
            }
        }

        public static MonitoringLogResponse Map(this MonitoringLog row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                Order = row.Order,
                EndDate = row.EndDate,
                InitialDate = row.InitialDate,

                ProjectId = row.ProjectId,
            };
        }

    }
}
