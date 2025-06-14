using Shared.Models.Constrainsts.Requests;
using Shared.Models.Constrainsts.Responses;

namespace Shared.Models.Constrainsts.Mappers
{
    public static class ConstrainstMapper
    {
        public static ChangeConstrainstOrderDowmRequest ToDown(this ConstrainstResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeConstrainstOrderUpRequest ToUp(this ConstrainstResponse response)
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
