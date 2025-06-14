using Shared.Commons;
using Shared.Models.IdentityModels.Requests.Identity;

namespace Server.Interfaces.Identity
{
    public interface IAccountService
    {
        Task<Shared.Commons.IResult> UpdateProfileAsync(UpdateProfileRequest model, string userId);

        Task<Shared.Commons.IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId);

        Task<IResult<string>> GetProfilePictureAsync(string userId);

        Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);
    }
}