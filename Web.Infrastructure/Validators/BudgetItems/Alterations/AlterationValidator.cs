using Shared.Models.BudgetItems.IndividualItems.Alterations.Requests;
using Shared.Models.BudgetItems.IndividualItems.Alterations.Responses;
using Shared.Models.BudgetItems.IndividualItems.Alterations.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.BudgetItems.Alterations
{
    
    public class AlterationValidator : AbstractValidator<AlterationResponse>
    {
        private readonly IGenericService Service;

        public AlterationValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            RuleFor(x => x.UnitaryCost).GreaterThan(0).WithMessage("Unitary cost must be defined!");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be defined!");

            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(AlterationResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateAlterationRequest validate = new()
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
