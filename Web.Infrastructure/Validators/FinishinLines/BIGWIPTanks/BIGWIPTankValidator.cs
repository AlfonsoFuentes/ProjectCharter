using Shared.Models.FinishingLines.BIGWIPTanks;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.FinishinLines.BIGWIPTanks
{

    public class BIGWIPTankValidator : AbstractValidator<BIGWIPTankResponse>
    {
        private readonly IGenericService Service;

        public BIGWIPTankValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");

            RuleFor(x => x.CapacityValue).GreaterThan(0).WithMessage("Capacity must be defined!");
            RuleFor(x => x.Backbone).NotNull().WithMessage("BackBone must be defined!");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(BIGWIPTankResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateBIGWIPTankNameRequest validate = new()
            {
                Name = name,

                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
