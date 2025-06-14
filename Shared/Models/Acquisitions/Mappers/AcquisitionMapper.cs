using Shared.Models.Acquisitions.Requests;
using Shared.Models.Acquisitions.Responses;

namespace Shared.Models.Acquisitions.Mappers
{
    public static class AcquisitionMapper
    {
        public static ChangeAcquisitionOrderDowmRequest ToDown(this AcquisitionResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeAcquisitionOrderUpRequest ToUp(this AcquisitionResponse response)
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
