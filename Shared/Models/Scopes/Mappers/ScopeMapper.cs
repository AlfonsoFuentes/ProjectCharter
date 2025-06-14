using Shared.Models.Scopes.Requests;
using Shared.Models.Scopes.Responses;

namespace Shared.Models.Scopes.Mappers
{
    public static class ScopeMapper
    {
        public static ChangeScopeOrderDowmRequest ToDown(this ScopeResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeScopeOrderUpRequest ToUp(this ScopeResponse response)
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
