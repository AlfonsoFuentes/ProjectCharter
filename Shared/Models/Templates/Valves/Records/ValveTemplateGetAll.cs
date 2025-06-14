using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Templates.Valves.Records
{
    public class ValveTemplateGetAll : IGetAll
    {
        public string EndPointName => StaticClass.ValveTemplates.EndPoint.GetAll;
    }
}
