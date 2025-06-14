using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Templates.Pipings.Responses;
using Shared.Models.Templates.Valves.Responses;

namespace Shared.Models.Templates.Pipings.Requests
{
    public class DeleteGroupPipeTemplatesRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of Pipe Templates";

        public override string ClassName => StaticClass.PipeTemplates.ClassName;

        public HashSet<PipeTemplateResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.PipeTemplates.EndPoint.DeleteGroup;
    }
}
