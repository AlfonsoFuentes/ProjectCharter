using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.ExpertJudgements.Responses
{
    public class ExpertJudgementResponseList : IResponseAll
    {
        public List<ExpertJudgementResponse> Items { get; set; } = new();
        public Guid ProjectId {  get; set; }
    }
}
