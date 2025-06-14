using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Requirements.Responses
{
    public class RequirementResponseList : IResponseAll
    {
        public List<RequirementResponse> Items { get; set; } = new();
        public Guid ProjectId { get; set; }
    }
}
