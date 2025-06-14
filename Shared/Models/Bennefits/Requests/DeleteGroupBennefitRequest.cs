using Shared.Models.Bennefits.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Bennefits.Requests
{
    public class DeleteGroupBennefitRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }

        public override string Legend => "Group of Bennefit";

        public override string ClassName => StaticClass.Bennefits.ClassName;

        public HashSet<BennefitResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Bennefits.EndPoint.DeleteGroup;
    }
}
