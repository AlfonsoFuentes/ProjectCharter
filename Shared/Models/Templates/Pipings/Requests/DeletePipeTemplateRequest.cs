using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Templates.Pipings.Requests
{
    public class DeletePipeTemplateRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.PipeTemplates.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.PipeTemplates.EndPoint.Delete;
    }
}
