using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Foundations.Validators
{
    public class ValidateFoundationRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.Foundations.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Foundations.ClassName;
    }

}
