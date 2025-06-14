using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Templates.Valves.Responses
{
    public class ValveTemplateResponseList : IResponseAll
    {
        public List<ValveTemplateResponse> Items { get; set; } = new();
    }
}
