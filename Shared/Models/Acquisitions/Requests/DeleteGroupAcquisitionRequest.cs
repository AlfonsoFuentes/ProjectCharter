using Shared.Models.Acquisitions.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Acquisitions.Requests
{
    public class DeleteGroupAcquisitionRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public override string Legend => "Group of Acquisition";

        public override string ClassName => StaticClass.Acquisitions.ClassName;

        public HashSet<AcquisitionResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Acquisitions.EndPoint.DeleteGroup;
    }
}
