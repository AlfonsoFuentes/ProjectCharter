using Shared.Models.FinishingLines.MixerBackbones;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.FinishinLines.MixerBackbones
{
   
    public class MixerBackboneValidator : AbstractValidator<MixerBackboneResponse>
    {
        private readonly IGenericService Service;

        public MixerBackboneValidator(IGenericService service)
        {
            Service = service;
            //RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            RuleFor(x => x.BatchCycleTimeValue).GreaterThan(0).WithMessage("Batch time must be defined!");
            RuleFor(x => x.Backbone).NotNull().WithMessage("BackBone must be defined!");
            RuleFor(x => x.CapacityValue).GreaterThan(0).WithMessage("Capacity must be defined!");
            //RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
            //    .When(x => !string.IsNullOrEmpty(x.Name))
            //    .WithMessage(x => $"{x.Name} already exist");

        }

        //async Task<bool> ReviewIfNameExist(MixerBackboneResponse request, string name, CancellationToken cancellationToken)
        //{
        //    ValidateMixerBackboneNameRequest validate = new()
        //    {
        //        Name = name,

        //        Id = request.Id

        //    };
        //    var result = await Service.Validate(validate);
        //    return !result;
        //}
    }
}
