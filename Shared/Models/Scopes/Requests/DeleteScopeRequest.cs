using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Scopes.Requests
{
    public class DeleteScopeRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;
        public Guid ProjectId { get; set; }
        public override string ClassName => StaticClass.Scopes.ClassName;
    
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Scopes.EndPoint.Delete;
    }
}
