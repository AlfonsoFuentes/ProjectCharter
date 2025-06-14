using Shared.Models.StakeHolders.Responses;
using Shared.Models.StakeHolders.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.StakeHolders
{
   
    public class StakeHolderValidator : AbstractValidator<StakeHolderResponse>
    {
        private readonly IGenericService Service;

        public StakeHolderValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email must be defined!");
            RuleFor(x => x.Area).NotEmpty().WithMessage("Area must be defined!");
            RuleFor(x => x.Email).EmailAddress().When(x=>!string.IsNullOrEmpty(x.Email)).WithMessage("Email must be valid!");
          
            //RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone Number must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(StakeHolderResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateStakeHolderRequest validate = new()
            {
                Name = name,
             
                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
       
    }
}
