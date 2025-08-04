using Shared.Models.FinishingLines.Products;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.FinishinLines.Products
{
    public class ProductValidator : AbstractValidator<ProductResponse>
    {
        private readonly IGenericService Service;

        public ProductValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            RuleFor(x => x.Components.Count).NotEqual(0).WithMessage("At least one component must be defined!");
       
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");
            RuleFor(x => x.SumPorcentages).Equal(100)
              .WithMessage("The sum of the components percentages must be equal to 100%");
        }

        async Task<bool> ReviewIfNameExist(ProductResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateProductNameRequest validate = new()
            {
                Name = name,

                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
