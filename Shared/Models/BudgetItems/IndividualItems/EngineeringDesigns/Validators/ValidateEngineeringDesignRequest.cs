using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.EngineeringDesigns.Validators
{
    public class ValidateEngineeringDesignRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.EngineeringDesigns.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.EngineeringDesigns.ClassName;
    }

}
