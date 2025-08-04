using Shared.Models.FinishingLines.InitialLevelBigWips;
using Shared.Models.FinishingLines.ProductionPlans;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.FinishinLines.InitialLevelBigWips
{
    public class InitialLevelBigWipValidator : AbstractValidator<InitialLevelBigWipResponse>
    {


        public InitialLevelBigWipValidator()
        {

            RuleFor(x => x.InitialLevelValue).LessThanOrEqualTo(x => x.BigWipTank.CapacityValue).WithMessage("Initial level Must be lower than capacity");


        }


    }
}
