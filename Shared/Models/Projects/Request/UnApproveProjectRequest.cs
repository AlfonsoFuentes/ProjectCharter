using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Projects.Request
{
    public class UnApproveProjectRequest : UpdateMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClasses.StaticClass.Projects.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClasses.StaticClass.Projects.EndPoint.UnApprove;
    }
}
