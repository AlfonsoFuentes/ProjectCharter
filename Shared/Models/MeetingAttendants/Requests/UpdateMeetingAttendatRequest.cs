using Shared.Enums.Meetings;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.StakeHolders.Responses;

namespace Shared.Models.MeetingAttendants.Requests
{
    public class UpdateMeetingAttendantRequest : UpdateMessageResponse, IRequest
    {
        public StakeHolderResponse StakeHolder { get; set; } = null!;
        public string StakeHolderName => StakeHolder == null ? string.Empty : StakeHolder.Name;
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.MeetingAttendants.EndPoint.Update;
        public Guid ProjectId { get; set; }
        public Guid MeetingId { get; set; }
        public override string Legend => StakeHolderName;

        public override string ClassName => StaticClass.MeetingAttendants.ClassName;
    }
}
