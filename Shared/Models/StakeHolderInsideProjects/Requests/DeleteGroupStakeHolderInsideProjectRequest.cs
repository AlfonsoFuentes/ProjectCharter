using Shared.Models.StakeHolderInsideProjects.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.StakeHolderInsideProjects.Requests
{
    public class DeleteGroupStakeHolderInsideProjectRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public override string Legend => "Group of StakeHolderInsideProject";

        public override string ClassName => StaticClass.StakeHolderInsideProjects.ClassName;

        public HashSet<StakeHolderInsideProjectResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.StakeHolderInsideProjects.EndPoint.DeleteGroup;
    }
}
