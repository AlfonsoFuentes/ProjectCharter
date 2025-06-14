namespace MudBlazorWeb.Services.Identities.Authentications
{
    public interface IAuthenticationManager : IManagetAuth
    {
        Task<IResult<TokenResponse>> Login(TokenRequest model);

        Task<IResult> Logout();

        Task<string> RefreshToken();

        Task<string> TryRefreshToken();

        Task<string> TryForceRefreshToken();

      
    }
}