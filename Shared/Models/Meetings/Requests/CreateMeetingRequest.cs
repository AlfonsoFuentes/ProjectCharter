using Shared.Enums.Meetings;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Meetings.Requests
{
    public class CreateMeetingRequest : CreateMessageResponse, IRequest
    {
        public DateTime? DateofMeeting { get; set; } = DateTime.UtcNow;
        public MeetingTypeEnum MeetingType { get; set; } = MeetingTypeEnum.None;
        public string Subject { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EndPointName => StaticClass.Meetings.EndPoint.Create;
        public Guid ProjectId { get; set; }
        public override string Legend => Name;

        public override string ClassName => StaticClass.Meetings.ClassName;

    }

}
