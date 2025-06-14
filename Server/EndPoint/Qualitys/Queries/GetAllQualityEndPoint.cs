using Shared.Models.Qualitys.Records;
using Shared.Models.Qualitys.Responses;

namespace Server.EndPoint.Qualitys.Queries
{
    public static class GetAllQualityEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Qualitys.EndPoint.GetAll, async (QualityGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetQualityAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<QualityResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Qualitys.ClassLegend));
                    }

                    var maps = FilterQuality(request, rows);

                    var response = new QualityResponseList
                    {
                        Items = maps,
                       ProjectId = request.ProjectId,
                    };

                    return Result<QualityResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetQualityAsync(QualityGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.Qualitys);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.Qualitys.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<QualityResponse> FilterQuality(QualityGetAll request, Project project)
            {
                return  project.Qualitys.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}