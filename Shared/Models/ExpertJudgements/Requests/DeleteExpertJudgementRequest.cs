using Shared.Models.FileResults.Generics.Request;
using Shared.Models.StakeHolders.Responses;

namespace Shared.Models.ExpertJudgements.Requests
{
    public class DeleteExpertJudgementRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;
        public StakeHolderResponse? Expert { get; set; }
        public override string ClassName => StaticClass.ExpertJudgements.ClassName;
     
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.ExpertJudgements.EndPoint.Delete;
    }
}
