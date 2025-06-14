using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.Instruments;
using Shared.Enums.Materials;
using Shared.Enums.NozzleTypes;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Responses;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Validators;
using Shared.Models.BudgetItems.IndividualItems.Instruments.Validators;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.BudgetItems.BasicInstruments
{

    public class BasicInstrumentValidator : AbstractValidator<BasicInstrumentResponse>
    {
        private readonly IGenericService Service;

        public BasicInstrumentValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            RuleFor(x => x.BudgetUSD).GreaterThan(0).When(x => !x.IsExisting).WithMessage("Budget must be defined!");

            RuleFor(x => x.BudgetUSD).GreaterThan(0).When(x => !x.IsExisting).WithMessage("Budget must be defined!");

            RuleFor(x => x.TagLetter).NotEmpty().When(x => x.ShowDetails)
              .WithMessage("Tag Letter must be defined!");

            RuleFor(x => x.Template.VariableInstrument).NotEqual(VariableInstrumentEnum.None).When(x => x.ShowDetails)
             .WithMessage("Variable must be defined!");

            RuleFor(x => x.ProvisionalTag).NotEmpty().When(x => x.ShowProvisionalTag && !x.ShowDetails)
            .WithMessage("Tag must be defined!");

            RuleFor(x => x.Template.ModifierVariable).NotEqual(ModifierVariableInstrumentEnum.None).When(x => x.ShowDetails)
        .WithMessage("Modifier Variable must be defined!");


            RuleFor(x => x.Template.Reference).NotEmpty().When(x => x.ShowDetails)
            .WithMessage("Reference must be defined!");

            RuleFor(x => x.Template.Diameter).NotEqual(NominalDiameterEnum.None).When(x => x.ShowDetails)
           .WithMessage("Diameter must be defined!");

            RuleFor(x => x.Template.ConnectionType).NotEqual(ConnectionTypeEnum.None).When(x => x.ShowDetails)
             .WithMessage("Connection Type must be defined!");

            RuleFor(x => x.Template.Brand).NotNull().When(x => x.ShowDetails)
             .WithMessage("Brand must be defined!");

            RuleFor(x => x.Template.Material).NotEqual(MaterialEnum.None).When(x => x.ShowDetails)
          .WithMessage("Internal Material must be defined!");

            RuleFor(x => x.Template.Model).NotEmpty().When(x => x.ShowDetails)
          .WithMessage("Model must be defined!");


            RuleFor(x => x.Nozzles).Must(ReviewInlet).When(x => x.ShowDetails
            && (x.Template.VariableInstrument.Id != VariableInstrumentEnum.VolumeFlowMeter.Id || x.Template.VariableInstrument.Id != VariableInstrumentEnum.MassFlowMeter.Id))
                .WithMessage("Nozzles must be have one inlet");

            RuleFor(x => x.Nozzles).Must(ReviewInletOutlet).When(x => x.ShowDetails
              && (x.Template.VariableInstrument.Id == VariableInstrumentEnum.VolumeFlowMeter.Id || x.Template.VariableInstrument.Id == VariableInstrumentEnum.MassFlowMeter.Id))
                  .WithMessage("Nozzles must be have one inlet and one outlet");

            RuleFor(x => x.Nozzles).Must(ReviewConnectionType).When(x => x.ShowDetails)
               .WithMessage("All connection types nozzle must be defined!");

            RuleFor(x => x.Nozzles).Must(ReviewDiameter).When(x => x.ShowDetails)
                .WithMessage("All diameter nozzle must be defined!");

            RuleFor(x => x.TagNumber).NotEmpty().When(x => x.ShowDetails)
                .WithMessage("Tag Number must be defined!");

            RuleFor(x => x.Tag)
               .MustAsync(ReviewIfTagExist)
               .When(x => x.ShowDetails && !x.ShowProvisionalTag)
              .WithMessage(x => $"{x.Tag} already exist");

            RuleFor(x => x.ProvisionalTag)
                .MustAsync(ReviewIfTagExist)
                .When(x => !x.ShowDetails && x.ShowProvisionalTag)
               .WithMessage(x => $"{x.ProvisionalTag} already exist");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

      
        }
      
        async Task<bool> ReviewIfNameExist(BasicInstrumentResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateBasicInstrumentRequest validate = new()
            {
                Name = name,
                ProjectId = request.ProjectId,
                Id = request.Id,


            };
            var result = await Service.Validate(validate);
            return !result;
        }
        async Task<bool> ReviewIfTagExist(BasicInstrumentResponse request, string tag, CancellationToken cancellationToken)
        {
            ValidateBasicInstrumentTagRequest validate = new()
            {

                Id = request.Id,
                Tag = tag,
                ProjectId = request.ProjectId,


            };
            var result = await Service.Validate(validate);
            return !result;
        }
        bool ReviewInletOutlet(List<NozzleResponse> nozzles)
        {
            if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Inlet.Id)) return false;
            if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Outlet.Id)) return false;

            return true;
        }
        bool ReviewInlet(List<NozzleResponse> nozzles)
        {
            if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Inlet.Id)) return false;

            return true;
        }
        bool ReviewConnectionType(List<NozzleResponse> nozzles)
        {
            if (nozzles.Any(x => x.ConnectionType.Id == ConnectionTypeEnum.None.Id)) return false;

            return true;
        }
        bool ReviewDiameter(List<NozzleResponse> nozzles)
        {
            if (nozzles.Any(x => x.NominalDiameter.Id == NominalDiameterEnum.None.Id)) return false;

            return true;
        }
    }
}
