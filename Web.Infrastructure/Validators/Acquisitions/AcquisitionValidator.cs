using Shared.Models.Acquisitions.Responses;
using Shared.Models.Acquisitions.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Acquisitions
{
  
    public class AcquisitionValidator : AbstractValidator<AcquisitionResponse>
    {
        private readonly IGenericService Service;

        public AcquisitionValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(AcquisitionResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateAcquisitionRequest validate = new()
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
