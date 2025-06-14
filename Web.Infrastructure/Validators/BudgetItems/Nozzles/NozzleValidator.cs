using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.NozzleTypes;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;

namespace Web.Infrastructure.Validators.BudgetItems.Nozzles
{
    public class NozzleValidator : AbstractValidator<NozzleResponse>
    {
        public NozzleValidator()
        {
            RuleFor(x => x.ConnectionType).NotEqual(ConnectionTypeEnum.None).WithMessage("Connection Type must be defined!");
            RuleFor(x => x.NominalDiameter).NotEqual(NominalDiameterEnum.None).WithMessage("Diameter must be defined!");
            RuleFor(x => x.NozzleType).NotEqual(NozzleTypeEnum.None).WithMessage("Nozzle Type must be defined!");
        }
    }
}
