using Shared.Models.FinishingLines.WIPTankLines;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.FinishinLines.WIPTankLines
{
   
    public class WIPTankLineValidator : AbstractValidator<WIPTankLineResponse>
    {
        private readonly IGenericService Service;

        public WIPTankLineValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
 
            RuleFor(x => x.CapacityValue).GreaterThan(0).WithMessage("Capacity must be defined!");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(WIPTankLineResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateWIPTankLineNameRequest validate = new()
            {
                Name = name,

                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
