using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.StakeHolderInsideProjects.Responses
{
    public class StakeHolderInsideProjectResponseList : IResponseAll
    {
        public List<StakeHolderInsideProjectResponse> Items { get; set; } = new();
        public Guid ProjectId { get; set; } 
    }
}
