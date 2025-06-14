using Shared.Models.KnownRisks.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.KnownRisks.Requests
{
    public class DeleteGroupKnownRiskRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public override string Legend => "Group of KnownRisk";

        public override string ClassName => StaticClass.KnownRisks.ClassName;

        public HashSet<KnownRiskResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.KnownRisks.EndPoint.DeleteGroup;
    }
}
