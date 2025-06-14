using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MeetingAgreements.Requests
{
    public class CreateMeetingAgreementRequest : CreateMessageResponse, IRequest
    {
        public Guid MeetingId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string EndPointName => StaticClass.MeetingAgreements.EndPoint.Create;
        public Guid ProjectId { get; set; }
        public override string Legend => Name;

        public override string ClassName => StaticClass.MeetingAgreements.ClassName;

    }

}
