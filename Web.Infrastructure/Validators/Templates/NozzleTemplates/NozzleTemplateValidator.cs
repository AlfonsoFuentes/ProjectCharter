using FluentValidation;
using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Enums.NozzleTypes;
using Shared.Models.Templates.NozzleTemplates;

namespace Web.Infrastructure.Validators.Templates.NozzleTemplates
{
    public class NozzleTemplateValidator : AbstractValidator<NozzleTemplateResponse>
    {
        public NozzleTemplateValidator()
        {
            RuleFor(x => x.ConnectionType).NotEqual(ConnectionTypeEnum.None).WithMessage("Connection Type must be defined!");
            RuleFor(x => x.NominalDiameter).NotEqual(NominalDiameterEnum.None).WithMessage("Diameter must be defined!");
            RuleFor(x => x.NozzleType).NotEqual(NozzleTypeEnum.None).WithMessage("Nozzle Type must be defined!");
        }
    }
}
