using static Shared.StaticClasses.StaticClass;

namespace Shared.Models.MeetingAgreements.Responses
{
    public class MeetingAgreementResponse : BaseResponse
    {
        public Guid ProjectId { get; set; }
    
        public Guid MeetingId { get; set; }
    }
}
