using Server.EndPoint.MonitoringLogs.Queries;
using Shared.Models.MonitoringLogs.Records;
using Shared.Models.MonitoringLogs.Responses;

namespace Server.EndPoint.MonitoringLogs.Queries
{
    public static class GetAllMonitoringLogEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MonitoringLogs.EndPoint.GetAll, async (MonitoringLogGetAll request, IQueryRepository repository) =>
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
                        ProjectId = request.ProjectId,
                    };

                    return Result<MonitoringLogResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetMonitoringLogAsync(MonitoringLogGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.MonitoringLogs);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.MonitoringLogs.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<MonitoringLogResponse> FilterMonitoringLog(MonitoringLogGetAll request, Project project)
            {
                return project.MonitoringLogs.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}