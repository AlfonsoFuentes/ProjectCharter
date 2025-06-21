using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.OtherTasks.Responses
{
    public class OtherTaskResponseList : IResponseAll
    {
        public List<OtherTaskResponse> Items { get; set; } = new();
        public Guid ProjectId { get; set; }
    }
}
