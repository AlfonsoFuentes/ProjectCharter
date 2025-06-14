using Shared.Enums.Meetings;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MeetingAgreements.Requests
{
    public class UpdateMeetingAgreementRequest : UpdateMessageResponse, IRequest
    {

        public DateTime? DateofSuggestion { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.MeetingAgreements.EndPoint.Update;
        public Guid MeetingId { get; set; }
        public Guid ProjectId { get; set; }
        public override string Legend => Name;

        public override string ClassName => StaticClass.MeetingAgreements.ClassName;
    }
}
