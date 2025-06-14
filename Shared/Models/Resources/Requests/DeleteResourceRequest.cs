using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Resources.Requests
{
    public class DeleteResourceRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.Resources.ClassName;
      
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Resources.EndPoint.Delete;
    }
}
