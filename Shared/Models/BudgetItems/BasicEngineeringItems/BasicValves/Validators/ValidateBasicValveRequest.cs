using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Validators
{
    public class ValidateBasicValveRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.BasicValves.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.BasicValves.ClassName;
    }

}
