using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.StakeHolderInsideProjects.Requests
{
    public class DeleteStakeHolderInsideProjectRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public override string Legend => Name;
        public override string ClassName => StaticClass.StakeHolders.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.StakeHolderInsideProjects.EndPoint.Delete;
    }
}
