using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Templates.Instruments.Records
{

    public class GetInstrumentTemplateByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.InstrumentTemplates.EndPoint.GetById;
        public override string ClassName => StaticClass.InstrumentTemplates.ClassName;
    }
}
