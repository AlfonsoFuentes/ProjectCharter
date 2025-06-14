using Shared.Models.Bennefits.Requests;
using Shared.Models.Bennefits.Responses;

namespace Shared.Models.Bennefits.Mappers
{
    public static class BennefitMapper
    {
        public static ChangeBennefitOrderDowmRequest ToDown(this BennefitResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeBennefitOrderUpRequest ToUp(this BennefitResponse response)
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
