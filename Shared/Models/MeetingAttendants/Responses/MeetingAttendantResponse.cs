using Shared.Models.StakeHolders.Responses;
using static Shared.StaticClasses.StaticClass;

namespace Shared.Models.MeetingAttendants.Responses
{
    public class MeetingAttendantResponse : BaseResponse
    {
        public Guid ProjectId { get; set; }
        public StakeHolderResponse StakeHolder { get; set; } = null!;

        public string StakeholderName => StakeHolder == null ? string.Empty : StakeHolder.Name;
        public string StakeholderArea => StakeHolder == null ? string.Empty : StakeHolder.Area;
        public Guid MeetingId { get; set; }
    }
}
