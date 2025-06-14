using Server.Database.Entities.ProjectManagements;
using Shared.Models.LearnedLessons.Requests;

namespace Server.EndPoint.LearnedLessons.Commands
{
    public static class DeleteLearnedLessonEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.LearnedLessons.EndPoint.Delete, async (DeleteLearnedLessonRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<LearnedLesson>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }


                    List<string> cache = [ .. StaticClass.LearnedLessons.Cache.Key(row.Id, row.ProjectId)];

                    await Repository.RemoveAsync(row);
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }

}
