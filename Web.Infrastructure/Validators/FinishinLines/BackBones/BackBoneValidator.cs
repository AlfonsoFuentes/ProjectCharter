using Shared.Models.FinishingLines.BackBones;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.FinishinLines.BackBones
{
   
    public class BackBoneValidator : AbstractValidator<BackBoneResponse>
    {
        private readonly IGenericService Service;

        public BackBoneValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(BackBoneResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateBackBoneNameRequest validate = new()
            {
                Name = name,

                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
