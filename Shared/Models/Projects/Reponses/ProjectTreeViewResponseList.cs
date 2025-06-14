using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Projects.Reponses
{
    public class ProjectTreeViewResponseList: IResponseAll
    {
        public List<ProjectTreeViewResponse> Items { get; set; } = new();
    }
}
