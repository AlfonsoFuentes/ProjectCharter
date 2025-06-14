using System.ComponentModel.DataAnnotations;

namespace Shared.Models.IdentityModels.Requests.Identity
{
    public class TokenRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}