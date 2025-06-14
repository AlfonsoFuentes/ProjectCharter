using Shared.Models.Scopes.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Scopes.Requests
{
    public class DeleteGroupScopeRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }

        public override string Legend => "Group of Scope";

        public override string ClassName => StaticClass.Scopes.ClassName;

        public HashSet<ScopeResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Scopes.EndPoint.DeleteGroup;
    }
}
