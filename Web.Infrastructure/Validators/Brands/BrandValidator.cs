using Shared.Models.Brands.Responses;
using Shared.Models.Brands.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Brands
{
    public class BrandValidator : AbstractValidator<BrandResponse>
    {
        private readonly IGenericService Service;

        public BrandValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(BrandResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateBrandRequest validate = new()
            {
                Name = name,

                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
