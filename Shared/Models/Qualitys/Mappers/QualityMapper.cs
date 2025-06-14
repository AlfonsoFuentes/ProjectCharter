using Shared.Models.Qualitys.Requests;
using Shared.Models.Qualitys.Responses;

namespace Shared.Models.Qualitys.Mappers
{
    public static class QualityMapper
    {
        public static ChangeQualityOrderDowmRequest ToDown(this QualityResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeQualityOrderUpRequest ToUp(this QualityResponse response)
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
