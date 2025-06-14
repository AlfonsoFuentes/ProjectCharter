using Server.Database.Entities.ProjectManagements;
using Shared.Models.LearnedLessons.Requests;
using Shared.Models.LearnedLessons.Responses;

namespace Server.EndPoint.LearnedLessons.Commands
{

    public static class CreateUpdateLearnedLessonEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.LearnedLessons.EndPoint.CreateUpdate, async (LearnedLessonResponse Data, IRepository Repository) =>
                {
                    LearnedLesson? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<LearnedLesson, Project>(Data.ProjectId);

                        row = LearnedLesson.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<LearnedLesson>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }
          

                    Data.Map(row);
                    List<string> cache = [.. StaticClass.LearnedLessons.Cache.Key(row.Id, row.ProjectId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        static LearnedLesson Map(this LearnedLessonResponse request, LearnedLesson row)
        {
            row.Name = request.Name;
            return row;
        }

    }

}
