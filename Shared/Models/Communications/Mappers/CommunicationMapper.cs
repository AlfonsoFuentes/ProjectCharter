using Shared.Models.Communications.Requests;
using Shared.Models.Communications.Responses;

namespace Shared.Models.Communications.Mappers
{
    public static class CommunicationMapper
    {
        public static ChangeCommunicationOrderDowmRequest ToDown(this CommunicationResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeCommunicationOrderUpRequest ToUp(this CommunicationResponse response)
        {
            return new()
            {
                ProjectId = response.ProjectId,
                Id = response.Id,
                Name = response.Name,
                Order = response.Order,
            };
        }
       
    }

}
