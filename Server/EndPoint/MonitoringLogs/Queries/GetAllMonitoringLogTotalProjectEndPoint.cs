using Shared.Models.MonitoringLogs.Records;
using Shared.Models.MonitoringLogs.Responses;

namespace Server.EndPoint.MonitoringLogs.Queries
{
    public static class GetAllMonitoringLogTotalProjectEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MonitoringLogs.EndPoint.GetAllTotalProjects, async (TotalMonitoringLogGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetMonitoringLogAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<MonitoringLogResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.MonitoringLogs.ClassLegend));
                    }

                    var maps = FilterMonitoringLog(request, rows);

                    var response = new MonitoringLogResponseList
                    {
                        Items = maps,

                    };

                    return Result<MonitoringLogResponseList>.Success(response);
                });
            }

            private static async Task<List<Project>> GetMonitoringLogAsync(TotalMonitoringLogGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.MonitoringLogs);

                string cacheKey = StaticClass.MonitoringLogs.Cache.GetAllTotalProjects;

                return await repository.GetAllAsync(Cache: cacheKey, Includes: includes);
            }

            private static List<MonitoringLogResponse> FilterMonitoringLog(TotalMonitoringLogGetAll request, List<Project> projects)
            {
                return projects.SelectMany(x => x.MonitoringLogs).OrderBy(x => x.InitialDate!.Value).Select(ac => ac.Map()).ToList();
            }
        }
    }
}