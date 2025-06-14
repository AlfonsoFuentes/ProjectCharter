using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Templates.Pipings.Responses
{
    public class PipeTemplateResponseList : IResponseAll
    {
        public List<PipeTemplateResponse> Items { get; set; } = new();
    }
}
