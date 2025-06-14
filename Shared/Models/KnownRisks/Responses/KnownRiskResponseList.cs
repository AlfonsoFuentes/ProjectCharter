using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.KnownRisks.Responses
{
    public class KnownRiskResponseList : IResponseAll
    {
        public List<KnownRiskResponse> Items { get; set; } = new();
        public Guid ProjectId { get; set; }
    }
}
