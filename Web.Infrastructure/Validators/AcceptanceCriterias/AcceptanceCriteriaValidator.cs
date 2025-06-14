using Shared.Models.AcceptanceCriterias.Requests;
using Shared.Models.AcceptanceCriterias.Responses;
using Shared.Models.AcceptanceCriterias.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.AcceptanceCriterias
{
  
    public class AcceptanceCriteriaValidator : AbstractValidator<AcceptanceCriteriaResponse>
    {
        private readonly IGenericService Service;

        public AcceptanceCriteriaValidator(IGenericService service)
        {
            Service = service;
                  RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(AcceptanceCriteriaResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateAcceptanceCriteriaRequest validate = new()
            {
                Name = name,
                ProjectId = request.ProjectId,
                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
