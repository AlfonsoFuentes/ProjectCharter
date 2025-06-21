using Shared.Models.OtherTasks.Responses;
using Shared.Models.OtherTasks.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.OtherTasks
{
  
    public class OtherTaskValidator : AbstractValidator<OtherTaskResponse>
    {
        private readonly IGenericService Service;

        public OtherTaskValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            RuleFor(x => x.EndDate).NotEmpty().When(x => x.Id != Guid.Empty)
            .WithMessage("Closing text must defined");
            RuleFor(x => x.StartDate).NotNull().When(x => x.Id == Guid.Empty)
               .WithMessage("Start date must defined");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(OtherTaskResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateOtherTaskRequest validate = new()
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
