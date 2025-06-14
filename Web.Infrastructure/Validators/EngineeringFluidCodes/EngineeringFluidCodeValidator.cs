using Shared.Models.EngineeringFluidCodes.Requests;
using Shared.Models.EngineeringFluidCodes.Responses;
using Shared.Models.EngineeringFluidCodes.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.EngineeringFluidCodes
{
  
    public class EngineeringFluidCodeValidator : AbstractValidator<EngineeringFluidCodeResponse>
    {
        private readonly IGenericService Service;

        public EngineeringFluidCodeValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code must be defined!");
            RuleFor(x => x.Code).Length(1, 3).WithMessage("The fluid code cannot be more than 3 characters");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(EngineeringFluidCodeResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateEngineeringFluidCodeRequest validate = new()
            {
                Name = name,
                Code=request.Code,
                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
