using Shared.Models.ExpertJudgements.Records;

namespace Server.EndPoint.ExpertJudgements.Queries
{
    public static class GetAllExpertJudgementEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ExpertJudgements.EndPoint.GetAll, async (ExpertJudgementGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetExpertJudgementAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<ExpertJudgementResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.ExpertJudgements.ClassLegend));
                    }

                    var maps = FilterExpertJudgement(request, rows);

                    var response = new ExpertJudgementResponseList
                    {
                        Items = maps,
                        ProjectId = request.ProjectId,
                    };

                    return Result<ExpertJudgementResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetExpertJudgementAsync(ExpertJudgementGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.ExpertJudgements).ThenInclude(x=>x.Expert!);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.ExpertJudgements.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<ExpertJudgementResponse> FilterExpertJudgement(ExpertJudgementGetAll request, Project project)
            {
                return  project.ExpertJudgements.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}