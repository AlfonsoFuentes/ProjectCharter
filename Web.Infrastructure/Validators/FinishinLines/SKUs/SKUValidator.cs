using Shared.Models.FinishingLines.SKUs;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.FinishinLines.SKUs
{
   
    public class SKUValidator : AbstractValidator<SKUResponse>
    {
        private readonly IGenericService Service;

        public SKUValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            RuleFor(x => x.VolumePerEAValue).GreaterThan(0).WithMessage("Volume/EA must be defined!");
            RuleFor(x => x.MassPerEAValue).GreaterThan(0).WithMessage("Mass/EA must be defined!");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");
         
            RuleFor(x => x.Product).NotNull().WithMessage("Product must be defined!");
          
        }

        async Task<bool> ReviewIfNameExist(SKUResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateSKUNameRequest validate = new()
            {
                Name = name,

                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
