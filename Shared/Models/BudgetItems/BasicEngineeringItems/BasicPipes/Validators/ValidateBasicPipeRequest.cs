using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Validators
{
    public class ValidateBasicPipeRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.BasicPipes.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.BasicPipes.ClassName;
    }

}
