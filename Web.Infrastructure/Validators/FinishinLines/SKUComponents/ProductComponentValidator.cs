using Shared.Models.FinishingLines.ProductComponents;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.FinishinLines.SKUComponents
{
   
    public class ProductComponentValidator : AbstractValidator<ProductComponentResponse>
    {
        private readonly IGenericService Service;

        public ProductComponentValidator(IGenericService service)
        {
            Service = service;
            //RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
        
            RuleFor(x => x.Backbone).NotNull().WithMessage("Backbone must be defined!");
            RuleFor(x => x.Percentage).Must(x => x > 0).WithMessage("Percentage must be greater than 0");
            //RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
            //    .When(x => !string.IsNullOrEmpty(x.Name))
            //    .WithMessage(x => $"{x.Name} already exist");

        }

        //async Task<bool> ReviewIfNameExist(SKUComponentResponse request, string name, CancellationToken cancellationToken)
        //{
        //    ValidateSKUComponentNameRequest validate = new()
        //    {
        //        Name = name,

        //        Id = request.Id

        //    };
        //    var result = await Service.Validate(validate);
        //    return !result;
        //}
    }
}
