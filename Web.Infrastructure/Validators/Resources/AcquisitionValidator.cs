using Shared.Models.Resources.Responses;
using Shared.Models.Resources.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Resources
{
  
    public class ResourceValidator : AbstractValidator<ResourceResponse>
    {
        private readonly IGenericService Service;

        public ResourceValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(ResourceResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateResourceRequest validate = new()
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
