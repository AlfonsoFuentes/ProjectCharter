using Server.Interfaces.Identity;
using Shared.Constants.Routes;
using Shared.Models.IdentityModels.Requests.Identity;


namespace Server.EndPoint.Authentications
{
    public static class RegisterUserEndPoint
    {


        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(AccountEndpoints.User.Register, async (RegisterRequest request, HttpRequest header, IUserService _userService) =>
                {
                    var origin = header.Headers["origin"];
                    var response = await _userService.RegisterAsync(request, origin!);
                    return response;

        
                });
            }
        }


    }
}
