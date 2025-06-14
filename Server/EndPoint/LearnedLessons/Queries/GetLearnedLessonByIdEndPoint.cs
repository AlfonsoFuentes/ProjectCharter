using Server.Database.Entities.ProjectManagements;
using Shared.Models.LearnedLessons.Records;
using Shared.Models.LearnedLessons.Responses;

namespace Server.EndPoint.LearnedLessons.Queries
{
    public static class GetLearnedLessonByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.LearnedLessons.EndPoint.GetById, async (GetLearnedLessonByIdRequest request, IQueryRepository Repository) =>
                {
                   

                    Expression<Func<LearnedLesson, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.LearnedLessons.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map();
                    return Result.Success(response);

                });
            }
        }

        public static LearnedLessonResponse Map(this LearnedLesson row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                ProjectId = row.ProjectId,
                Order=row.Order,
                
                

            };
        }

    }
}
