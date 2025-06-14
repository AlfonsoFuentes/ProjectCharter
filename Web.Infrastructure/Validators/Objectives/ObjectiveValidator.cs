using Shared.Models.Objectives.Responses;
using Shared.Models.Objectives.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Objectives
{
  
    public class ObjectiveValidator : AbstractValidator<ObjectiveResponse>
    {
        private readonly IGenericService Service;

        public ObjectiveValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(ObjectiveResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateObjectiveRequest validate = new()
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
