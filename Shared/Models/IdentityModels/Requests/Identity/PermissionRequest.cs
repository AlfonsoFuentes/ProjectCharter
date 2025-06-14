using System.Collections.Generic;

namespace Shared.Models.IdentityModels.Requests.Identity
{
    public class PermissionRequest
    {
        public string? RoleId { get; set; }
        public List<RoleClaimRequest> RoleClaims { get; set; } = null!;
    }
}