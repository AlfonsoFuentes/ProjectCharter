using Server.Interfaces.Identity;
using Shared.Constants.Routes;
using Shared.Models.IdentityModels.Requests.Identity;


namespace Server.EndPoint.Authentications
{
    public static class GetRegisteredUserEndPoint
    {


        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(AccountEndpoints.User.GetUser, async (GetUserRequest request, HttpRequest header, IUserService _userService) =>
                {
                 
                    var response = await _userService.GetAsync(request.UserId);
                    return response;


                });
            }
        }


    }
}
