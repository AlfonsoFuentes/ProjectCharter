using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Structurals.Validators
{
    public class ValidateStructuralRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.Structurals.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Structurals.ClassName;
    }

}
