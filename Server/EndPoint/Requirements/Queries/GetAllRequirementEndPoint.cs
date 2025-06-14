using Server.EndPoint.Requirements.Queries;
using Shared.Models.Requirements.Records;
using Shared.Models.Requirements.Responses;

namespace Server.EndPoint.Requirements.Queries
{
    public static class GetAllRequirementEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Requirements.EndPoint.GetAll, async (RequirementGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetRequirementAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<RequirementResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Requirements.ClassLegend));
                    }

                    var maps = FilterRequirement(request, rows);

                    var response = new RequirementResponseList
                    {
                        Items = maps,
                       ProjectId = request.ProjectId,

                    };

                    return Result<RequirementResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetRequirementAsync(RequirementGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.Requirements);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.Requirements.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<RequirementResponse> FilterRequirement(RequirementGetAll request, Project project)
            {
                return  project.Requirements.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}