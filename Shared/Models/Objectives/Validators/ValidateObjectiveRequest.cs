using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Objectives.Validators
{
    public class ValidateObjectiveRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.Objectives.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Objectives.ClassName;
    }
    
}
