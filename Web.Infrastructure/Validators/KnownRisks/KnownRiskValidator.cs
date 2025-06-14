using Shared.Models.KnownRisks.Requests;
using Shared.Models.KnownRisks.Responses;
using Shared.Models.KnownRisks.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.KnownRisks
{
    public class KnownRiskValidator : AbstractValidator<KnownRiskResponse>
    {
        private readonly IGenericService Service;

        public KnownRiskValidator(IGenericService service)
        {
            Service = service;
                  RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(KnownRiskResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateKnownRiskRequest validate = new()
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
