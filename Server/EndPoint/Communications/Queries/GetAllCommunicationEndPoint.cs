using Server.EndPoint.Communications.Queries;
using Shared.Models.Communications.Records;
using Shared.Models.Communications.Responses;

namespace Server.EndPoint.Communications.Queries
{
    public static class GetAllCommunicationEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Communications.EndPoint.GetAll, async (CommunicationGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetCommunicationAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<CommunicationResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Communications.ClassLegend));
                    }

                    var maps = FilterCommunication(request, rows);

                    var response = new CommunicationResponseList
                    {
                        Items = maps,
                        ProjectId = request.ProjectId,
                    };

                    return Result<CommunicationResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetCommunicationAsync(CommunicationGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.Communications);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.Communications.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<CommunicationResponse> FilterCommunication(CommunicationGetAll request, Project project)
            {
                return project.Communications.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}