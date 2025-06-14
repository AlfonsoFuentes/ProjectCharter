using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Meetings.Validators
{
  
    public class ValidateMeetingRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.Meetings.EndPoint.Validate;
        public Guid ProjectId { get; set; }
        public override string Legend => Name;

        public override string ClassName => StaticClass.Meetings.ClassName;
    }
}
