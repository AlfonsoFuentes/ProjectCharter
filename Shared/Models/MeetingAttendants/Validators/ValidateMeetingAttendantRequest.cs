using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MeetingAttendants.Validators
{

    public class ValidateMeetingAttendantRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public Guid StakeHolderId { get; set; }
        public Guid MeetingId { get; set; }
        public string EndPointName => StaticClass.MeetingAttendants.EndPoint.Validate;

        public override string Legend => "Name";

        public override string ClassName => StaticClass.MeetingAttendants.ClassName;
    }
}
