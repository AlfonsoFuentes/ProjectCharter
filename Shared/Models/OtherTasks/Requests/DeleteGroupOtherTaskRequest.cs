using Shared.Models.OtherTasks.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.OtherTasks.Requests
{
    public class DeleteGroupOtherTaskRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public override string Legend => "Group of OtherTask";

        public override string ClassName => StaticClass.OtherTasks.ClassName;

        public HashSet<OtherTaskResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.OtherTasks.EndPoint.DeleteGroup;
    }
}
