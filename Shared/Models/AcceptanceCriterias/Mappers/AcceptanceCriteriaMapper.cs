using Shared.Models.AcceptanceCriterias.Requests;
using Shared.Models.AcceptanceCriterias.Responses;

namespace Shared.Models.AcceptanceCriterias.Mappers
{
    public static class AcceptanceCriteriaMapper
    {
        public static ChangeAcceptanceCriteriaOrderDowmRequest ToDown(this AcceptanceCriteriaResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeAcceptanceCriteriaOrderUpRequest ToUp(this AcceptanceCriteriaResponse response)
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
