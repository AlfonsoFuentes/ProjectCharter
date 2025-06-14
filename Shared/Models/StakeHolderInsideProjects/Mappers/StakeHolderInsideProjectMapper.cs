using Shared.Models.Backgrounds.Requests;
using Shared.Models.Backgrounds.Responses;
using Shared.Models.StakeHolderInsideProjects.Requests;
using Shared.Models.StakeHolderInsideProjects.Responses;
using System.Security.Cryptography.X509Certificates;

namespace Shared.Models.StakeHolderInsideProjects.Mappers
{
    public static class StakeHolderInsideProjectMapper
    {
        public static ChangeStakeHolderInsideProjectOrderDowmRequest ToDown(this StakeHolderInsideProjectResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeStakeHolderInsideProjectOrderUpRequest ToUp(this StakeHolderInsideProjectResponse response)
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
