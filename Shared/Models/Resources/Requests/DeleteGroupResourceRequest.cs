using Shared.Models.Resources.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Resources.Requests
{
    public class DeleteGroupResourceRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public override string Legend => "Group of Resource";

        public override string ClassName => StaticClass.Resources.ClassName;

        public HashSet<ResourceResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Resources.EndPoint.DeleteGroup;
    }
}
