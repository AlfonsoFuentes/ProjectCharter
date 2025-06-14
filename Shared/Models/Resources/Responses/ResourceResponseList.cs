using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Resources.Responses
{
    public class ResourceResponseList : IResponseAll
    {
        public List<ResourceResponse> Items { get; set; } = new();
        public Guid ProjectId { get; set; }
    }
}
