using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Requirements.Validators
{
   
    public class ValidateRequirementRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
   
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.Requirements.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Requirements.ClassName;
    }
}
