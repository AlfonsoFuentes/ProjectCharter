using Shared.Models.Assumptions.Requests;
using Shared.Models.Assumptions.Responses;
using Shared.Models.Assumptions.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Assumptions
{
  
    public class AssumptionValidator : AbstractValidator<AssumptionResponse>
    {
        private readonly IGenericService Service;

        public AssumptionValidator(IGenericService service)
        {
            Service = service;
                  RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(AssumptionResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateAssumptionRequest validate = new()
            {
                Name = name,
    
                 ProjectId= request.ProjectId,
                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
