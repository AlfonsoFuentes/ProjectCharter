using Shared.Models.Assumptions.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Assumptions.Requests
{
    public class DeleteGroupAssumptionRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public override string Legend => "Group of Assumption";

        public override string ClassName => StaticClass.Assumptions.ClassName;

        public HashSet<AssumptionResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Assumptions.EndPoint.DeleteGroup;
    }
}
