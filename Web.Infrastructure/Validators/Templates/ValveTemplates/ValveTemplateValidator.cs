using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Enums.NozzleTypes;
using Shared.Enums.ValvesEnum;
using Shared.Models.Brands.Responses;
using Shared.Models.Templates.NozzleTemplates;
using Shared.Models.Templates.Valves.Responses;
using Shared.Models.Templates.Valves.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Templates.ValveTemplates
{

    public class ValveTemplateValidator : AbstractValidator<ValveTemplateResponse>
    {
        private readonly IGenericService Service;

        public ValveTemplateValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Value).GreaterThan(0).WithMessage("Value must be defined!");
            RuleFor(x => x.Type).NotEqual(ValveTypesEnum.None).WithMessage("Type must be defined!");
        
            RuleFor(x => x.Model).NotEmpty().WithMessage("Model must be defined!");
            RuleFor(x => x.Material).NotEqual(MaterialEnum.None).WithMessage("Material must be defined!");
            RuleFor(x => x.ActuatorType).NotEqual(ActuatorTypeEnum.None).WithMessage("Actuator Type Material must be defined!");
            RuleFor(x => x.PositionerType).NotEqual(PositionerTypeEnum.None).When(x => x.ActuatorType != ActuatorTypeEnum.Hand).WithMessage("Positioner Type must be defined!");
            RuleFor(x => x.Diameter).NotEqual(NominalDiameterEnum.None).WithMessage("Diameter must be defined!");
            RuleFor(x => x.FailType).NotEqual(FailTypeEnum.None).When(x => x.ActuatorType != ActuatorTypeEnum.Hand).WithMessage("Fail Type must be defined!");
            RuleFor(x => x.SignalType).NotEqual(SignalTypeEnum.None).When(x => x.ActuatorType != ActuatorTypeEnum.Hand).WithMessage("Signal Type must be defined!");
            RuleFor(x => x.ConnectionType).NotEqual(ConnectionTypeEnum.None).WithMessage("Connection type must be defined!");

            RuleFor(x => x.Brand).NotNull().WithMessage("Brand must be defined!");

            RuleFor(x => x.Nozzles).Must(ReviewNozzles).WithMessage("Nozzles is not completed defined");



            RuleFor(x => x.Nozzles).Must(ReviewConnectionType)
               .WithMessage("All connection types nozzle must be defined!");

            RuleFor(x => x.Nozzles).Must(ReviewDiameter)
                .WithMessage("All diameter nozzle must be defined!");

            RuleFor(x => x.Brand).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.BrandName))
                .WithMessage(x => $"Template already exist");

        }

        async Task<bool> ReviewIfNameExist(ValveTemplateResponse request, BrandResponse? brand, CancellationToken cancellationToken)
        {
            ValidateValveTemplateRequest validate = new()
            {

             
                Brand = request.Brand!.Name,
                Model = request.Model,
                ActuadorType = request.ActuatorType.Id,
                Diameter = request.Diameter.Id,
                FailType = request.FailType.Id,
                HasFeedBack = request.HasFeedBack,
                Material = request.Material.Id,
                PositionerType = request.PositionerType.Id,
                SignalType = request.SignalType.Id,
                Type = request.Type.Id,
           
                Id = request.Id,
                NozzleTemplates = request.Nozzles


            };
            var result = await Service.Validate(validate);
            return !result;
        }
        bool ReviewNozzles(ValveTemplateResponse request, List<NozzleTemplateResponse> nozzles)
        {
            if (request.Type.Id == ValveTypesEnum.Sample_port.Id)
            {
                return nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Inlet.Id);


            }

           
            if(request.Type.Id != ValveTypesEnum.Ball_Three_Way_L.Id || request.Type.Id != ValveTypesEnum.Ball_Three_Way_T.Id || request.Type.Id != ValveTypesEnum.Ball_Four_Way.Id)
            {
                if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Inlet.Id)) return false;
                if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Outlet.Id)) return false;

            }
            else if (request.Type.Id == ValveTypesEnum.Ball_Four_Way.Id)
            {
                if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Inlet.Id)) return false;
                if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Outlet.Id)) return false;
                if (nozzles.Count < 4) return false;
            }
            else
            {
                if (nozzles.Count < 3) return false;
            }

                return true;
        }
        bool ReviewInletOutlet(List<NozzleTemplateResponse> nozzles)
        {
            if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Inlet.Id)) return false;
            if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Outlet.Id)) return false;
            return true;
        }
        bool ReviewThreeWayInletOutlet(List<NozzleTemplateResponse> nozzles)
        {
            if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Inlet.Id)) return false;
            if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Outlet.Id)) return false;
            if (nozzles.Count < 3) return false;

            return true;
        }
        bool ReviewFourWayInletOutlet(List<NozzleTemplateResponse> nozzles)
        {
            if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Inlet.Id)) return false;
            if (!nozzles.Any(x => x.NozzleType.Id == NozzleTypeEnum.Outlet.Id)) return false;
            if (nozzles.Count < 4) return false;

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
