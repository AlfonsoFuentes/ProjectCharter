using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Acquisitions.Responses
{
    public class AcquisitionResponseList : IResponseAll
    {
        public List<AcquisitionResponse> Items { get; set; } = new();
        public Guid ProjectId { get; set; }
    }
}
