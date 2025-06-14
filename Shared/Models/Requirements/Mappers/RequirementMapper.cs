using Shared.Models.Requirements.Requests;
using Shared.Models.Requirements.Responses;

namespace Shared.Models.Requirements.Mappers
{
    public static class RequirementMapper
    {
        public static ChangeRequirementOrderDowmRequest ToDown(this RequirementResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeRequirementOrderUpRequest ToUp(this RequirementResponse response)
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
