using Server.EndPoint.OtherTasks.Queries;
using Shared.Models.OtherTasks.Records;
using Shared.Models.OtherTasks.Responses;

namespace Server.EndPoint.OtherTasks.Queries
{
    public static class GetAllByProjectOtherTaskEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.OtherTasks.EndPoint.GetAllByProject, async (OtherTaskGetAllByProject request, IQueryRepository repository) =>
                {
                    var rows = await GetOtherTaskAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<OtherTaskResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.OtherTasks.ClassLegend));
                    }

                    var maps = FilterOtherTask(request, rows);

                    var response = new OtherTaskResponseList
                    {
                        Items = maps,
                        ProjectId = request.ProjectId,
                    };

                    return Result<OtherTaskResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetOtherTaskAsync(OtherTaskGetAllByProject request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.OtherTasks);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.OtherTasks.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<OtherTaskResponse> FilterOtherTask(OtherTaskGetAllByProject request, Project project)
            {
                return project.OtherTasks.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}