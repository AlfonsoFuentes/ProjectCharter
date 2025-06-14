using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MeetingAgreements.Validators
{
  
    public class ValidateMeetingAgreementRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Guid MeetingId { get; set; }
        public string EndPointName => StaticClass.MeetingAgreements.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.MeetingAgreements.ClassName;
    }
}
