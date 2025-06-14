using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Backgrounds.Responses
{
    public class BackGroundResponseList: IResponseAll
    {
        public List<BackGroundResponse> Items { get; set; } = new();
        public Guid ProjectId { get; set; } 

    }
}
