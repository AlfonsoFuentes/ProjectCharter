using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.AcceptanceCriterias.Validators
{
   
    public class ValidateAcceptanceCriteriaRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.AcceptanceCriterias.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.AcceptanceCriterias.ClassName;
    }
}
