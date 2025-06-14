using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Communications.Responses
{
    public class CommunicationResponseList : IResponseAll
    {
        public List<CommunicationResponse> Items { get; set; } = new();
        public Guid  ProjectId{ get; set; } 
    }
}
