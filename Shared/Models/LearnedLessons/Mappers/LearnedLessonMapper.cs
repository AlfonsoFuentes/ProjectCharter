using Shared.Models.LearnedLessons.Requests;
using Shared.Models.LearnedLessons.Responses;

namespace Shared.Models.LearnedLessons.Mappers
{
    public static class LearnedLessonMapper
    {
        public static ChangeLearnedLessonOrderDowmRequest ToDown(this LearnedLessonResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeLearnedLessonOrderUpRequest ToUp(this LearnedLessonResponse response)
        {
            return new()
            {
                ProjectId = response.ProjectId,
                Id = response.Id,
                Name = response.Name,
                Order = response.Order,
            };
        }
   
    }

}
