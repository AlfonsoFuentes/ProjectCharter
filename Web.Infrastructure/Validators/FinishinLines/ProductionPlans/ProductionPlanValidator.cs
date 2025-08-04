using FluentValidation;
using Shared.Models.FinishingLines.ProductionPlans;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.FinishinLines.ProductionPlans
{
   
    public class ProductionPlanValidator : AbstractValidator<ProductionPlanResponse>
    {
        private readonly IGenericService Service;

        public ProductionPlanValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            
            RuleFor(x => x.LineAssignments.Count).GreaterThan(0).WithMessage("Assigment must be defined!");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(ProductionPlanResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateProductionPlanNameRequest validate = new()
            {
                Name = name,

                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
