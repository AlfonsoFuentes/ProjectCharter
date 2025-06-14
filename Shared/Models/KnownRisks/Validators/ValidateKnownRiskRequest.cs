using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.KnownRisks.Validators
{
  
    public class ValidateKnownRiskRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.KnownRisks.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.KnownRisks.ClassName;
    }
}
