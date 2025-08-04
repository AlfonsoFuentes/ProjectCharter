using Shared.Models.FinishingLines.ProductionLines;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.FinishinLines.ProductionLines
{
   
    public class ProductionLineValidator : AbstractValidator<ProductionLineResponse>
    {
        private readonly IGenericService Service;

        public ProductionLineValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");

            RuleFor(x => x.WIPTanks.Count).GreaterThan(0).WithMessage("Wip tanks must be defined!");
            RuleFor(x => x.LineSpeeds.Count).GreaterThan(0).WithMessage("Line speed per sku must be defined!");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(ProductionLineResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateProductionLineNameRequest validate = new()
            {
                Name = name,

                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
