using Shared.Models.LearnedLessons.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.LearnedLessons.Requests
{
    public class DeleteGroupLearnedLessonRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }

        public override string Legend => "Group of LearnedLesson";

        public override string ClassName => StaticClass.LearnedLessons.ClassName;

        public HashSet<LearnedLessonResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.LearnedLessons.EndPoint.DeleteGroup;
    }
}
