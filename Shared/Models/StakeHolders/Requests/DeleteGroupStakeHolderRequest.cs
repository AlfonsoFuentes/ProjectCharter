using Shared.Models.FileResults.Generics.Request;
using Shared.Models.StakeHolders.Responses;

namespace Shared.Models.StakeHolders.Requests
{
    public class DeleteGroupStakeHolderRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of StakeHolder";

        public override string ClassName => StaticClass.StakeHolders.ClassName;

        public HashSet<StakeHolderResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.StakeHolders.EndPoint.DeleteGroup;
    }
}
