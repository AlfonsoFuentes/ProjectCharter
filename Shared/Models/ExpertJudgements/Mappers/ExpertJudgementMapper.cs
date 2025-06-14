using Shared.Models.ExpertJudgements.Requests;
using Shared.Models.ExpertJudgements.Responses;

namespace Shared.Models.ExpertJudgements.Mappers
{
    public static class ExpertJudgementMapper
    {
        public static ChangeExpertJudgementOrderDowmRequest ToDown(this ExpertJudgementResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeExpertJudgementOrderUpRequest ToUp(this ExpertJudgementResponse response)
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
