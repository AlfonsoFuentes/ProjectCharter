using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Templates.Valves.Responses;

namespace Shared.Models.Templates.Valves.Requests
{
    public class DeleteGroupValveTemplatesRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of Valve Templates";

        public override string ClassName => StaticClass.ValveTemplates.ClassName;

        public HashSet<ValveTemplateResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.ValveTemplates.EndPoint.DeleteGroup;
    }
}
