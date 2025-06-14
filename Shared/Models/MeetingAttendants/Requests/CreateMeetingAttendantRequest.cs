using Shared.Models.FileResults.Generics.Request;
using Shared.Models.StakeHolders.Responses;

namespace Shared.Models.MeetingAttendants.Requests
{
    public class CreateMeetingAttendantRequest : CreateMessageResponse, IRequest
    {
        
        public Guid MeetingId { get; set; }
        public StakeHolderResponse? StakeHolder { get; set; } = null!;
        public string StakeHolderName => StakeHolder == null ? string.Empty : StakeHolder.Name;
        public string EndPointName => StaticClass.MeetingAttendants.EndPoint.Create;
        public Guid ProjectId { get; set; }
        public override string Legend => StakeHolderName;

        public override string ClassName => StaticClass.MeetingAttendants.ClassName;

    }

}
