using Shared.Models.OtherTasks.Requests;
using Shared.Models.OtherTasks.Responses;

namespace Shared.Models.OtherTasks.Mappers
{
    public static class OtherTaskMapper
    {
        public static ChangeOtherTaskOrderDowmRequest ToDown(this OtherTaskResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeOtherTaskOrderUpRequest ToUp(this OtherTaskResponse response)
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
