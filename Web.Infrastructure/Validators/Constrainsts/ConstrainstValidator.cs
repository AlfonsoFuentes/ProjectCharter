using Shared.Models.Constrainsts.Requests;
using Shared.Models.Constrainsts.Responses;
using Shared.Models.Constrainsts.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Constrainsts
{
  
    public class ConstrainstValidator : AbstractValidator<ConstrainstResponse>
    {
        private readonly IGenericService Service;

        public ConstrainstValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(ConstrainstResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateConstrainstRequest validate = new()
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
