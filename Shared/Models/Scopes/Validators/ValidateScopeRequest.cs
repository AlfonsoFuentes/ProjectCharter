using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Scopes.Validators
{
 
    public class ValidateScopeRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.Scopes.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Scopes.ClassName;
    }
}
