using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Templates.Instruments.Requests
{
    public class DeleteInstrumentTemplateRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.InstrumentTemplates.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.InstrumentTemplates.EndPoint.Delete;
    }
}
