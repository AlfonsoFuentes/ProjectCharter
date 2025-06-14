using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MeetingAgreements.Requests
{
    public class DeleteMeetingAgreementRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.MeetingAgreements.ClassName;
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.MeetingAgreements.EndPoint.Delete;
    }
}
