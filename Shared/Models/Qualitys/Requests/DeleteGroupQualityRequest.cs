using Shared.Models.Qualitys.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Qualitys.Requests
{
    public class DeleteGroupQualityRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public override string Legend => "Group of Quality";

        public override string ClassName => StaticClass.Qualitys.ClassName;

        public HashSet<QualityResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Qualitys.EndPoint.DeleteGroup;
    }
}
