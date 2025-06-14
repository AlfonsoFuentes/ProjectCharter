using Shared.Commons;
using Shared.Models.IdentityModels.Requests.Identity;
using Shared.Models.IdentityModels.Responses.Identity;

namespace Server.Interfaces.Identity
{
    public interface ITokenService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}