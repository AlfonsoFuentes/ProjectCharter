using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Templates.Valves.Records
{

    public class GetValveTemplateByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.ValveTemplates.EndPoint.GetById;
        public override string ClassName => StaticClass.ValveTemplates.ClassName;
    }
}
