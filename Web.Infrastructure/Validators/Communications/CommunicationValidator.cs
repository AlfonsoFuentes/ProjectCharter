using Shared.Models.Communications.Responses;
using Shared.Models.Communications.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Communications
{
  
    public class CommunicationValidator : AbstractValidator<CommunicationResponse>
    {
        private readonly IGenericService Service;

        public CommunicationValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(CommunicationResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateCommunicationRequest validate = new()
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
