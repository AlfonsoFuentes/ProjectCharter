using FluentValidation;
using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.Instruments;
using Shared.Enums.Materials;
using Shared.Enums.NozzleTypes;
using Shared.Models.Brands.Responses;
using Shared.Models.Templates.Instruments.Requests;
using Shared.Models.Templates.Instruments.Responses;
using Shared.Models.Templates.Instruments.Validators;
using Shared.Models.Templates.NozzleTemplates;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Templates.InstrumentTemplates
{

    public class InstrumentTemplateValidator : AbstractValidator<InstrumentTemplateResponse>
    {
        private readonly IGenericService Service;

        public InstrumentTemplateValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Value).GreaterThan(0).WithMessage("Value must be defined!");
            RuleFor(x => x.TagLetter).NotEmpty()
              .WithMessage("Tag Letter must be defined!");
            RuleFor(x => x.VariableInstrument).NotEmpty()
           .WithMessage("Variable must be defined!");

            RuleFor(x => x.ModifierVariable).NotEmpty()
        .WithMessage("Modifier Variable must be defined!");

           
            RuleFor(x => x.Material).NotEqual(MaterialEnum.None)
          .WithMessage("Material must be defined!");


            RuleFor(x => x.ConnectionType).NotEqual(ConnectionTypeEnum.None)
             .WithMessage("Connection Type must be defined!");

            RuleFor(x => x.Diameter).NotEqual(NominalDiameterEnum.None)
       .WithMessage("Diameter type must be defined!");

            RuleFor(x => x.Model).NotEmpty()
          .WithMessage("Model must be defined!");

            RuleFor(x => x.Reference).NotEmpty()
          .WithMessage("Reference must be defined!");

            RuleFor(x => x.Nozzles).Must(ReviewInlet).When(x =>
               (x.VariableInstrument.Id != VariableInstrumentEnum.VolumeFlowMeter.Id || x.VariableInstrument.Id != VariableInstrumentEnum.MassFlowMeter.Id))
                   .WithMessage("Nozzles must be have one inlet");

            RuleFor(x => x.Nozzles).Must(ReviewInletOutlet).When(x =>
               (x.VariableInstrument.Id == VariableInstrumentEnum.VolumeFlowMeter.Id || x.VariableInstrument.Id == VariableInstrumentEnum.MassFlowMeter.Id))
                  .WithMessage("Nozzles must be have one inlet and one outlet");

            RuleFor(x => x.Nozzles).Must(ReviewConnectionType)
               .WithMessage("All connection types nozzle must be defined!");

            RuleFor(x => x.Nozzles).Must(ReviewDiameter)
                .WithMessage("All diameter nozzle must be defined!");



            RuleFor(x => x.Brand).NotNull().WithMessage("Brand must be defined!");

            RuleFor(x => x.Brand).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.BrandName))
                .WithMessage(x => $"Template already exist");



        }

        async Task<bool> ReviewIfNameExist(InstrumentTemplateResponse request, BrandResponse? brand, CancellationToken cancellationToken)
        {
            ValidateInstrumentTemplateRequest validate = new()
            {
          
                Brand = request.BrandName,
                Model = request.Model,
                Material = request.Material.Id,
                SignalType = request.SignalType.Id,
                VariableInstrument = request.VariableInstrument.Id,
                ModifierVariable = request.ModifierVariable.Id,             
                Reference = request.Reference,
                Id = request.Id,                  
                NozzleTemplates=request.Nozzles,

            };
            var result = await Service.Validate(validate);
            return !result;
        }
        bool ReviewInletOutlet(List<NozzleTemplateResponse> nozzles)
        {
            if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Inlet.Id)) return false;
            if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Outlet.Id)) return false;

            return true;
        }
        bool ReviewInlet(List<NozzleTemplateResponse> nozzles)
        {
            if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Inlet.Id)) return false;

            return true;
        }
        bool ReviewConnectionType(List<NozzleTemplateResponse> nozzles)
        {
            if (nozzles.Any(x => x.ConnectionType.Id == ConnectionTypeEnum.None.Id)) return false;

            return true;
        }
        bool ReviewDiameter(List<NozzleTemplateResponse> nozzles)
        {
            if (nozzles.Any(x => x.NominalDiameter.Id == NominalDiameterEnum.None.Id)) return false;

            return true;
        }
    }
}
