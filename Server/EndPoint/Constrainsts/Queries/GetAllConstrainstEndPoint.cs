using Server.EndPoint.Constrainsts.Queries;
using Shared.Models.Constrainsts.Records;
using Shared.Models.Constrainsts.Responses;

namespace Server.EndPoint.Constrainsts.Queries
{
    public static class GetAllConstrainstEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Constrainsts.EndPoint.GetAll, async (ConstrainstGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetConstrainstAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<ConstrainstResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Constrainsts.ClassLegend));
                    }

                    var maps = FilterConstrainst(request, rows);

                    var response = new ConstrainstResponseList
                    {
                        Items = maps,
                        ProjectId = request.ProjectId,
                    };

                    return Result<ConstrainstResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetConstrainstAsync(ConstrainstGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.Constrainsts);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.Constrainsts.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<ConstrainstResponse> FilterConstrainst(ConstrainstGetAll request, Project project)
            {
                return  project.Constrainsts.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}