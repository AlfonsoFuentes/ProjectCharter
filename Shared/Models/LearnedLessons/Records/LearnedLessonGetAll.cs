using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.LearnedLessons.Records
{
    public class LearnedLessonGetAll : IGetAll
    {
       
        public string EndPointName => StaticClass.LearnedLessons.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
    }
}
