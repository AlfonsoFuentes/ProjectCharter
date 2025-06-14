using Microsoft.AspNetCore.Identity;
using Server.Database.Identity;
using System.ComponentModel;

namespace Server.ExtensionsMethods
{
    public static class ClaimsHelper
    {


        public static async Task<IdentityResult> AddPermissionClaim(this RoleManager<IdentityRole> roleManager, 
            IdentityRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);

            return IdentityResult.Failed();
        }
    }
}