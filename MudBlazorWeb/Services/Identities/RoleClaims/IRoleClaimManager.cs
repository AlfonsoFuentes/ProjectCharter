using MudBlazorWeb.Services.Identities.Accounts;

namespace MudBlazorWeb.Services.Identities.RoleClaims
{
    public interface IRoleClaimManager : IManagetAuth
    {
        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsAsync();

        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsByRoleIdAsync(string roleId);

        Task<IResult<string>> SaveAsync(RoleClaimRequest role);

        Task<IResult<string>> DeleteAsync(string id);
    }
}