using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Validators
{
    public class ValidateBasicInstrumentRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.BasicInstruments.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.BasicInstruments.ClassName;
    }

}
