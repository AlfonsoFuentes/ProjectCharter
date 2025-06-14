using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.StakeHolders.Responses
{
    public class StakeHolderResponseList : IResponseAll
    {
        public List<StakeHolderResponse> Items { get; set; } = new();
    }
}
