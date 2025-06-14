using Shared.Models.BudgetItems.IndividualItems.EngineeringDesigns.Requests;
using Shared.Models.BudgetItems.IndividualItems.EngineeringDesigns.Responses;
using Shared.Models.BudgetItems.IndividualItems.EngineeringDesigns.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.BudgetItems.EngineeringDesigns
{
 
    public class EngineeringDesignValidator : AbstractValidator<EngineeringDesignResponse>
    {
        private readonly IGenericService Service;

        public EngineeringDesignValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            RuleFor(x => x.BudgetUSD).GreaterThan(0).WithMessage("Budget must be defined!");
            

            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(EngineeringDesignResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateEngineeringDesignRequest validate = new()
            {
                Name = name,
                ProjectId = request.ProjectId,
                Id = request.Id,
        

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
