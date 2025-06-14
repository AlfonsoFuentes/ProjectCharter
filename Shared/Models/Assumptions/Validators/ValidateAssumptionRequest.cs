using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Assumptions.Validators
{
 
    public class ValidateAssumptionRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.Assumptions.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Assumptions.ClassName;
    }
}
