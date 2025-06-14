using Shared.Models.Resources.Records;
using Shared.Models.Resources.Responses;

namespace Server.EndPoint.Resources.Queries
{
    public static class GetAllResourceEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Resources.EndPoint.GetAll, async (ResourceGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetResourceAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<ResourceResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Resources.ClassLegend));
                    }

                    var maps = FilterResource(request, rows);

                    var response = new ResourceResponseList
                    {
                        Items = maps,
                        ProjectId = request.ProjectId
                    };

                    return Result<ResourceResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetResourceAsync(ResourceGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.Resources);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.Resources.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<ResourceResponse> FilterResource(ResourceGetAll request, Project project)
            {
                return  project.Resources.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}