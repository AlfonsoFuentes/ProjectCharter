using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Templates.Valves.Requests
{
    public class DeleteValveTemplateRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.ValveTemplates.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.ValveTemplates.EndPoint.Delete;
    }
}
