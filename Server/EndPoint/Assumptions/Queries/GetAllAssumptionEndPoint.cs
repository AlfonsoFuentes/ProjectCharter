using Server.EndPoint.Assumptions.Queries;
using Shared.Models.Assumptions.Records;
using Shared.Models.Assumptions.Responses;

namespace Server.EndPoint.Assumptions.Queries
{
    public static class GetAllAssumptionEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Assumptions.EndPoint.GetAll, async (AssumptionGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetAssumptionAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<AssumptionResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Assumptions.ClassLegend));
                    }

                    var maps = FilterAssumption(request, rows);

                    var response = new AssumptionResponseList
                    {
                        Items = maps,
                        ProjectId = request.ProjectId,
                    };

                    return Result<AssumptionResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetAssumptionAsync(AssumptionGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.Assumptions);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.Assumptions.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<AssumptionResponse> FilterAssumption(AssumptionGetAll request, Project project)
            {
                return project.Assumptions.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}