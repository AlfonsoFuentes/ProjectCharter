using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.LearnedLessons.Requests
{
    public class DeleteLearnedLessonRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.LearnedLessons.ClassName;
      
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.LearnedLessons.EndPoint.Delete;
    }
}
