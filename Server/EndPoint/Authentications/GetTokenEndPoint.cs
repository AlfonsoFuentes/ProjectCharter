using Server.Interfaces.Identity;
using Shared.Constants.Routes;
using Shared.Models.IdentityModels.Requests.Identity;


namespace Server.EndPoint.Authentications
{
    public static class GetTokenEndPoint
    {


        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(AccountEndpoints.Token.Get, async (TokenRequest request, ITokenService _identityService) =>
                {
                    var response = await _identityService.LoginAsync(request);
                    return response;
                   
                });
            }
        }
      

    }
}
