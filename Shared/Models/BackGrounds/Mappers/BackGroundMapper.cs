using Shared.Models.Backgrounds.Requests;
using Shared.Models.Backgrounds.Responses;

namespace Shared.Models.BackGrounds.Mappers
{
    public static class BackGroundMapper
    {
        public static ChangeBackGroundOrderDowmRequest ToDown(this BackGroundResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeBackGroundOrderUpRequest ToUp(this BackGroundResponse response)
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
