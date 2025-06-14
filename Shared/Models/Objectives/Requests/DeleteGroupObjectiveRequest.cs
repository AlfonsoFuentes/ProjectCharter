using Shared.Models.Objectives.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Objectives.Requests
{
    public class DeleteGroupObjectiveRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public override string Legend => "Group of Objective";

        public override string ClassName => StaticClass.Objectives.ClassName;

        public HashSet<ObjectiveResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Objectives.EndPoint.DeleteGroup;
    }
}
