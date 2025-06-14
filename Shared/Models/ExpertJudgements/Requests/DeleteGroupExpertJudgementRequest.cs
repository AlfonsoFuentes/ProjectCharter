using Shared.Models.ExpertJudgements.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.ExpertJudgements.Requests
{
    public class DeleteGroupExpertJudgementRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }

        public override string Legend => "Group of ExpertJudgement";

        public override string ClassName => StaticClass.ExpertJudgements.ClassName;

        public HashSet<ExpertJudgementResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.ExpertJudgements.EndPoint.DeleteGroup;
    }
}
