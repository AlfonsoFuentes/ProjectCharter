﻿using Shared.Models.IdentityModels.Responses.Identity;
using System.Collections.Generic;

namespace Shared.Models.IdentityModels.Requests.Identity
{
    public class UpdateUserRolesRequest
    {
        public string UserId { get; set; } = string.Empty;
        public IList<UserRoleModel> UserRoles { get; set; } = null!;
    }
}