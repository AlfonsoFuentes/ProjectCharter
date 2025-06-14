using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Projects.Reponses
{
    public class ProjectResponseList : IResponseAll
    {
     
        public List<ProjectResponse> Items { get; set; } = new();
        public int LastOrder => Items.Count == 0 ? 1 : Items.MaxBy(x => x.Order)!.Order;


    }
}
