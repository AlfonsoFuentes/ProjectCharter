using Shared.Models.MeetingAgreements.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MeetingAgreements.Requests
{
    public class DeleteGroupMeetingAgreementRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public override string Legend => "Group of MeetingAgreement";

        public override string ClassName => StaticClass.MeetingAgreements.ClassName;

        public HashSet<MeetingAgreementResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.MeetingAgreements.EndPoint.DeleteGroup;
    }
}
