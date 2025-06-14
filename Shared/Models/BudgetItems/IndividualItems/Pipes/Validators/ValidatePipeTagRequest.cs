using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Pipes.Validators
{
    public class ValidatePipeTagRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }


        public string Tag { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.Pipes.EndPoint.ValidateTag;

        public override string Legend => Tag;

        public override string ClassName => StaticClass.Pipes.ClassName;
    }

}
