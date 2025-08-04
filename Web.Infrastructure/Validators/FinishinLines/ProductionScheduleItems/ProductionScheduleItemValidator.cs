using Shared.Models.FinishingLines.ProductionScheduleItems;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.FinishinLines.ProductionScheduleItems
{
   
    public class ProductionScheduleItemValidator : AbstractValidator<ProductionScheduleItemResponse>
    {
        private readonly IGenericService Service;

        public ProductionScheduleItemValidator(IGenericService service)
        {
            Service = service;
            //RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");

           
            RuleFor(x => x.Sku).NotNull().WithMessage("Sku must be defined!");
            RuleFor(x => x.MassPlannedValue).GreaterThan(0).WithMessage("Planned mass must be defined!");
           
            //RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
            //    .When(x => !string.IsNullOrEmpty(x.Name))
            //    .WithMessage(x => $"{x.Name} already exist");

        }

        //async Task<bool> ReviewIfNameExist(ProductionScheduleItemResponse request, string name, CancellationToken cancellationToken)
        //{
        //    ValidateProductionScheduleItemNameRequest validate = new()
        //    {
        //        Name = name,

        //        Id = request.Id

        //    };
        //    var result = await Service.Validate(validate);
        //    return !result;
        //}
    }
}
