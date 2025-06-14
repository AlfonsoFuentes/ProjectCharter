using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Templates.Instruments.Responses
{
    public class InstrumentTemplateResponseList : IResponseAll
    {
        public List<InstrumentTemplateResponse> Items { get; set; } = new();
    }
}
