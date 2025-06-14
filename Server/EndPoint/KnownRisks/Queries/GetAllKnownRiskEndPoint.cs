using Shared.Models.KnownRisks.Records;

namespace Server.EndPoint.KnownRisks.Queries
{
    public static class GetAllKnownRiskEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.KnownRisks.EndPoint.GetAll, async (KnownRiskGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetKnownRiskAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<KnownRiskResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.KnownRisks.ClassLegend));
                    }

                    var maps = FilterKnownRisk(request, rows);

                    var response = new KnownRiskResponseList
                    {
                        Items = maps,
                        ProjectId = request.ProjectId,
                    };

                    return Result<KnownRiskResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetKnownRiskAsync(KnownRiskGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.KnownRisks);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.KnownRisks.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<KnownRiskResponse> FilterKnownRisk(KnownRiskGetAll request, Project project)
            {
                return project.KnownRisks.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}