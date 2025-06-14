using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Bennefits.Validators
{

    public class ValidateBennefitRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.Bennefits.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Bennefits.ClassName;
    }
}
