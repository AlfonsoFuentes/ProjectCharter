using Shared.Models.AcceptanceCriterias.Records;
using Shared.Models.AcceptanceCriterias.Responses;

namespace Server.EndPoint.AcceptanceCriterias.Queries
{
    public static class GetAllAcceptanceCriteriaEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.AcceptanceCriterias.EndPoint.GetAll, async (AcceptanceCriteriaGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetAcceptanceCriteriaAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<AcceptanceCriteriaResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.AcceptanceCriterias.ClassLegend));
                    }

                    var maps = FilterAcceptanceCriteria(request, rows);

                    var response = new AcceptanceCriteriaResponseList
                    {
                        ProjectId = request.ProjectId,
                        Items = maps
                    };

                    return Result<AcceptanceCriteriaResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetAcceptanceCriteriaAsync(AcceptanceCriteriaGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.AcceptanceCriterias);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.AcceptanceCriterias.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<AcceptanceCriteriaResponse> FilterAcceptanceCriteria(AcceptanceCriteriaGetAll request, Project project)
            {
                return project.AcceptanceCriterias.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}