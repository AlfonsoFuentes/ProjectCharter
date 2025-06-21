using Shared.Models.OtherTasks.Records;
using Shared.Models.OtherTasks.Responses;

namespace Server.EndPoint.OtherTasks.Queries
{
    public static class GetAllOtherTaskEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.OtherTasks.EndPoint.GetAll, async (OtherTaskGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetOtherTaskAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<OtherTaskResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.OtherTasks.ClassLegend));
                    }
                    var tasks = rows.SelectMany(x => x.OtherTasks).ToList();
                    var maps = FilterOtherTask(rows);

                    var response = new OtherTaskResponseList
                    {
                        Items = maps,
                   
                    };

                    return Result<OtherTaskResponseList>.Success(response);
                });
            }

            private static async Task<List<Project>> GetOtherTaskAsync(OtherTaskGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.OtherTasks);
     
                string cacheKey = StaticClass.OtherTasks.Cache.GetAllProjects;

                return await repository.GetAllAsync(Cache: cacheKey, Includes: includes);
            }

            private static List<OtherTaskResponse> FilterOtherTask(List<Project> projects)
            {
                return projects.SelectMany(x => x.OtherTasks).OrderBy(x => x.CECName).ThenBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}