using Shared.Models.Scopes.Records;

namespace Server.EndPoint.Scopes.Queries
{
    public static class GetAllScopeEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Scopes.EndPoint.GetAll, async (ScopeGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetScopeAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<ScopeResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Scopes.ClassLegend));
                    }

                    var maps = FilterScope(request, rows);

                    var response = new ScopeResponseList
                    {
                        Items = maps,
                        ProjectId = request.ProjectId,
                    };

                    return Result<ScopeResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetScopeAsync(ScopeGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.Scopes);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.Scopes.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<ScopeResponse> FilterScope(ScopeGetAll request, Project project)
            {
                return  project.Scopes.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}