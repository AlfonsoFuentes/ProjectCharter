using Shared.Models.Requirements.Requests;
using Shared.Models.Requirements.Responses;
using Shared.Models.Requirements.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Requirements
{
  
    public class RequirementValidator : AbstractValidator<RequirementResponse>
    {
        private readonly IGenericService Service;

        public RequirementValidator(IGenericService service)
        {
            Service = service;
                  RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(RequirementResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateRequirementRequest validate = new()
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
