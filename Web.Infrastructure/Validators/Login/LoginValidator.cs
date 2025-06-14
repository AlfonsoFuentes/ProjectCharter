using Shared.Models.IdentityModels.Requests.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Infrastructure.Validators.Login
{
    public class LoginValidator : AbstractValidator<TokenRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Must supply valid email");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Must supply valid email");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Must supply valid password");
        }
    }
}
