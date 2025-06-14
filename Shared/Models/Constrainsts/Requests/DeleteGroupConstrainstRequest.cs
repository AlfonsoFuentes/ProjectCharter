using Shared.Models.Constrainsts.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Constrainsts.Requests
{
    public class DeleteGroupConstrainstRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public override string Legend => "Group of Constrainst";

        public override string ClassName => StaticClass.Constrainsts.ClassName;

        public HashSet<ConstrainstResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Constrainsts.EndPoint.DeleteGroup;
    }
}
