using Shared.Models.Objectives.Records;
using Shared.Models.Objectives.Responses;

namespace Server.EndPoint.Objectives.Queries
{
    public static class GetAllObjectiveEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Objectives.EndPoint.GetAll, async (ObjectiveGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetObjectiveAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<ObjectiveResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Objectives.ClassLegend));
                    }

                    var maps = FilterObjective(request, rows);

                    var response = new ObjectiveResponseList
                    {
                        Items = maps,
                        ProjectId = request.ProjectId,
                    };

                    return Result<ObjectiveResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetObjectiveAsync(ObjectiveGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.Objectives);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.Objectives.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<ObjectiveResponse> FilterObjective(ObjectiveGetAll request, Project project)
            {
                return project.Objectives.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }

}