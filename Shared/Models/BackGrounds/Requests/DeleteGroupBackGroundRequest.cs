using Shared.Models.Backgrounds.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Backgrounds.Requests
{
    public class DeleteGroupBackGroundRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public override string Legend => "Group of BackGround";

        public override string ClassName => StaticClass.BackGrounds.ClassName;

        public HashSet<BackGroundResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.BackGrounds.EndPoint.DeleteGroup;
    }
}
