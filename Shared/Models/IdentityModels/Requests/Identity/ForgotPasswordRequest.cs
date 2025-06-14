﻿using System.ComponentModel.DataAnnotations;

namespace Shared.Models.IdentityModels.Requests.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}