using Shared.Models.LearnedLessons.Validators;
using Shared.Models.Backgrounds.Validators;
using Server.Database.Entities.ProjectManagements;

namespace Server.EndPoint.LearnedLessons.Validators
{
    public static class ValidateLearnedLessonsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.LearnedLessons.EndPoint.Validate, async (ValidateLearnedLessonRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<LearnedLesson, bool>> CriteriaId = x => x.ProjectId == Data.ProjectId;
                    Func<LearnedLesson, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.LearnedLessons.Cache.GetAll(Data.ProjectId);

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
