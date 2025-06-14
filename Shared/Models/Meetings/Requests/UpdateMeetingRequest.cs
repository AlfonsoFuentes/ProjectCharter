using Shared.Enums.Meetings;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Meetings.Requests
{
    public class UpdateMeetingRequest : UpdateMessageResponse, IRequest
    {
        public DateTime? DateofMeeting { get; set; } = DateTime.UtcNow;
        public MeetingTypeEnum MeetingType { get; set; } = MeetingTypeEnum.None;
        public string Subject { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Meetings.EndPoint.Update;
        public Guid ProjectId { get; set; }
        public override string Legend => Name;

        public override string ClassName => StaticClass.Meetings.ClassName;
    }
}
