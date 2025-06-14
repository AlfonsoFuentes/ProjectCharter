using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.EngineeringFluidCodes.Validators
{
    public class ValidateEngineeringFluidCodeRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public string EndPointName => StaticClass.EngineeringFluidCodes.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.EngineeringFluidCodes.ClassName;
    }

}
