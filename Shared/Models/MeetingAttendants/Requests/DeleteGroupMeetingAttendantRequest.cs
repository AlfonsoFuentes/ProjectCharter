using Shared.Models.MeetingAttendants.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MeetingAttendants.Requests
{
    public class DeleteGroupMeetingAttendantRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of MeetingAttendant";

        public override string ClassName => StaticClass.MeetingAttendants.ClassName;

        public HashSet<MeetingAttendantResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.MeetingAttendants.EndPoint.DeleteGroup;
    }
}
