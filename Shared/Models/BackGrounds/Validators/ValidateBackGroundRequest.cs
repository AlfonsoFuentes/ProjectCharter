using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Backgrounds.Validators
{
   
    public class ValidateBackGroundRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.BackGrounds.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.BackGrounds.ClassName;
    }
}
