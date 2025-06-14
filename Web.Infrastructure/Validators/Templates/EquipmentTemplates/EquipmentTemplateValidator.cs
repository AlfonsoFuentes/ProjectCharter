using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Enums.NozzleTypes;
using Shared.Models.Brands.Responses;
using Shared.Models.Templates.Equipments.Requests;
using Shared.Models.Templates.Equipments.Responses;
using Shared.Models.Templates.Equipments.Validators;
using Shared.Models.Templates.NozzleTemplates;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Templates.EquipmentTemplates
{
    public class EquipmentTemplateValidator : AbstractValidator<EquipmentTemplateResponse>
    {
        private readonly IGenericService Service;

        public EquipmentTemplateValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Value).GreaterThan(0).WithMessage("Value must be defined!");
            RuleFor(x => x.Type).NotEmpty().WithMessage("Type must be defined!");
            RuleFor(x => x.TagLetter).NotEmpty().WithMessage("Tag Letter must be defined!");
            RuleFor(x => x.Model).NotEmpty().WithMessage("Model must be defined!");
            RuleFor(x => x.ExternalMaterial).NotEqual(MaterialEnum.None).WithMessage("External Material must be defined!");
            RuleFor(x => x.InternalMaterial).NotEqual(MaterialEnum.None).WithMessage("Internal Material must be defined!");
            RuleFor(x => x.Nozzles).Must(ReviewInletOutlet).WithMessage("Nozzles must be have one inlet and one outlet");

            RuleFor(x => x.Nozzles).Must(ReviewConnectionType)
               .WithMessage("All connection types nozzle must be defined!");

            RuleFor(x => x.Nozzles).Must(ReviewDiameter)
                .WithMessage("All diameter nozzle must be defined!");


            RuleFor(x => x.Brand).NotNull().WithMessage("Brand must be defined!");

            RuleFor(x => x.Brand).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.BrandName))
                .WithMessage(x => $"Template already exist");

        }

        async Task<bool> ReviewIfNameExist(EquipmentTemplateResponse request, BrandResponse? brand, CancellationToken cancellationToken)
        {
            ValidateEquipmentTemplateRequest validate = new()
            {
                 
               
                Brand = request.BrandName,
                Model = request.Model,
                InternalMaterial = request.InternalMaterial.Id,
                ExternalMaterial = request.ExternalMaterial.Id,
                Type = request.Type,
                SubType = request.SubType,
                TagLetter = request.TagLetter,
                Reference = request.Reference,
                NozzleTemplates=request.Nozzles,
                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
        bool ReviewInletOutlet(List<NozzleTemplateResponse> nozzles)
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
