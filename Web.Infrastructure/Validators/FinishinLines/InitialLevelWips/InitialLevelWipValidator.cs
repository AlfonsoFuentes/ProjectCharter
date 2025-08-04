using Shared.Models.FinishingLines.InitialLevelWips;

namespace Web.Infrastructure.Validators.FinishinLines.InitialLevelWips
{
    public class InitialLevelWipValidator : AbstractValidator<InitialLevelWipResponse>
    {


        public InitialLevelWipValidator()
        {



            RuleFor(x => x.InitialLevelValue).LessThanOrEqualTo(x => x.WipTankLine.CapacityValue).WithMessage("Initial level Must be lower than capacity");

        }


    }
}
