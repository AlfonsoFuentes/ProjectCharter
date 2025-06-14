using Server.EndPoint.LearnedLessons.Queries;
using Shared.Models.LearnedLessons.Records;
using Shared.Models.LearnedLessons.Responses;

namespace Server.EndPoint.LearnedLessons.Queries
{
    public static class GetAllLearnedLessonEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.LearnedLessons.EndPoint.GetAll, async (LearnedLessonGetAll request, IQueryRepository repository) =>
                {
                    var rows = await GetLearnedLessonAsync(request, repository);

                    if (rows == null)
                    {
                        return Result<LearnedLessonResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.LearnedLessons.ClassLegend));
                    }

                    var maps = FilterLearnedLesson(request, rows);

                    var response = new LearnedLessonResponseList
                    {
                        Items = maps,
                        ProjectId = request.ProjectId,
                    };

                    return Result<LearnedLessonResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetLearnedLessonAsync(LearnedLessonGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.LearnedLessons);
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.LearnedLessons.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }

            private static List<LearnedLessonResponse> FilterLearnedLesson(LearnedLessonGetAll request, Project project)
            {
                return  project.LearnedLessons.OrderBy(x => x.Order).Select(ac => ac.Map()).ToList();
            }
        }
    }
}