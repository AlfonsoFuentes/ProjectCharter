using Shared.Models.Communications.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Communications.Requests
{
    public class DeleteGroupCommunicationRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public override string Legend => "Group of Communication";

        public override string ClassName => StaticClass.Communications.ClassName;

        public HashSet<CommunicationResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Communications.EndPoint.DeleteGroup;
    }
}
