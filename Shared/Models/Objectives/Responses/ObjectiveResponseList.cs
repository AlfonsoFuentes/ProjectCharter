using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Objectives.Responses
{
    public class ObjectiveResponseList : IResponseAll
    {
        public List<ObjectiveResponse> Items { get; set; } = new();
        public Guid ProjectId { get; set; }
    }
}
