using Shared.Models.Requirements.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Requirements.Requests
{
    public class DeleteGroupRequirementRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }

        public override string Legend => "Group of Requirement";

        public override string ClassName => StaticClass.Requirements.ClassName;

        public HashSet<RequirementResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Requirements.EndPoint.DeleteGroup;
    }
}
