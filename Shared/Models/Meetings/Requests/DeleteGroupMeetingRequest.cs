using Shared.Models.Meetings.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Meetings.Requests
{
    public class DeleteGroupMeetingRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }

        public override string Legend => "Group of Meeting";

        public override string ClassName => StaticClass.Meetings.ClassName;

        public HashSet<MeetingResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Meetings.EndPoint.DeleteGroup;
    }
}
