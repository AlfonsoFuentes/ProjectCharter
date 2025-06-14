using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Resources.Validators
{
  
    public class ValidateResourceRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.Resources.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Resources.ClassName;
    }
}
