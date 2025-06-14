using Shared.Models.BudgetItems.IndividualItems.Taxs.Requests;
using Shared.Models.BudgetItems.IndividualItems.Taxs.Responses;
using Shared.Models.BudgetItems.IndividualItems.Taxs.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.BudgetItems.Taxs
{
 
    public class TaxValidator : AbstractValidator<TaxResponse>
    {
        private readonly IGenericService Service;

        public TaxValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");

            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                          .When(x => !string.IsNullOrEmpty(x.Name))
                          .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(TaxResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateTaxRequest validate = new()
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
