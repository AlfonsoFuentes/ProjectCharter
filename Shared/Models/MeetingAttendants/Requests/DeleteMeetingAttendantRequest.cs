using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MeetingAttendants.Requests
{
    public class DeleteMeetingAttendantRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.MeetingAttendants.ClassName;
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.MeetingAttendants.EndPoint.Delete;
    }
}
