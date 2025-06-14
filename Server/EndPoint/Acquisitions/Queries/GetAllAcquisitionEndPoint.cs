using Server.EndPoint.Acquisitions.Queries;
using Shared.Models.Acquisitions.Records;
using Shared.Models.Acquisitions.Responses;

namespace Server.EndPoint.Acquisitions.Queries
{
    public static class GetAllAcquisitionEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Acquisitions.EndPoint.GetAll, async (AcquisitionGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetAcquisitionAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<AcquisitionResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Acquisitions.ClassLegend));
                    }

                    var maps = FilterAcquisition(request, rows);

                    var response = new AcquisitionResponseList
                    {
                        Items = maps,
                        ProjectId = request.ProjectId,
                    };

                    return Result<AcquisitionResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetAcquisitionAsync(AcquisitionGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.Acquisitions);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.Acquisitions.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<AcquisitionResponse> FilterAcquisition(AcquisitionGetAll request, Project project)
            {
                return project.Acquisitions.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}