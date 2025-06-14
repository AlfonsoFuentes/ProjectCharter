using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Bennefits.Responses
{
    public class BennefitResponseList : IResponseAll
    {
        public List<BennefitResponse> Items { get; set; } = new();
        public Guid ProjectId { get; set; }
    }
}
