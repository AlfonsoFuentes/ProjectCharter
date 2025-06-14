using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Templates.Instruments.Records
{
    public class InstrumentTemplateGetAll : IGetAll
    {
        public string EndPointName => StaticClass.InstrumentTemplates.EndPoint.GetAll;
    }
}
