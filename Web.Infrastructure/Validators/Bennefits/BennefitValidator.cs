using Shared.Models.Bennefits.Requests;
using Shared.Models.Bennefits.Responses;
using Shared.Models.Bennefits.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Bennefits
{
  
    public class BennefitValidator : AbstractValidator<BennefitResponse>
    {
        private readonly IGenericService Service;

        public BennefitValidator(IGenericService service)
        {
            Service = service;
                  RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(BennefitResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateBennefitRequest validate = new()
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
