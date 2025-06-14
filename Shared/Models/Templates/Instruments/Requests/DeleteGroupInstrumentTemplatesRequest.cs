using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Templates.Instruments.Responses;

namespace Shared.Models.Templates.Instruments.Requests
{
    public class DeleteGroupInstrumentTemplatesRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of Instrument Templates";

        public override string ClassName => StaticClass.InstrumentTemplates.ClassName;

        public HashSet<InstrumentTemplateResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.InstrumentTemplates.EndPoint.DeleteGroup;
    }
}
