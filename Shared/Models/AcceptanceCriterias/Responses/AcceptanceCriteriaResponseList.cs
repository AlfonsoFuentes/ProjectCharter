using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.AcceptanceCriterias.Responses
{
    public class AcceptanceCriteriaResponseList : IResponseAll
    {
        public List<AcceptanceCriteriaResponse> Items { get; set; } = new();
        public Guid ProjectId { get; set; }
    }
}
