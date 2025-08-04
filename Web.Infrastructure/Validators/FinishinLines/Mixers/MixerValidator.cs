using Shared.Models.FinishingLines.Mixers;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.FinishinLines.Mixers
{
   
    public class MixerValidator : AbstractValidator<MixerResponse>
    {
        private readonly IGenericService Service;

        public MixerValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
          
            RuleFor(x => x.CleaningTimeValue).GreaterThan(0).WithMessage("Cleaning must be defined!");

            RuleFor(x => x.Capabilities.Count).GreaterThan(0).WithMessage("Mixer Capabilities must be defined!");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(MixerResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateMixerNameRequest validate = new()
            {
                Name = name,

                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
