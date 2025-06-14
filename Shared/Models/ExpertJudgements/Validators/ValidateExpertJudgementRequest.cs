using Shared.Models.FileResults.Generics.Request;
using Shared.Models.StakeHolders.Responses;

namespace Shared.Models.ExpertJudgements.Validators
{
  
    public class ValidateExpertJudgementRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public StakeHolderResponse? Expert {  get; set; }
        public string EndPointName => StaticClass.ExpertJudgements.EndPoint.Validate;
     
        public override string Legend => Name;

        public override string ClassName => StaticClass.ExpertJudgements.ClassName;
    }
}
