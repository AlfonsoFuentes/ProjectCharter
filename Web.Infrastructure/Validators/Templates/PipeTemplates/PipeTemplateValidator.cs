using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Models.Templates.Pipings.Responses;
using Shared.Models.Templates.Pipings.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Templates.PipeTemplates
{
  
    public class PipeTemplateValidator : AbstractValidator<PipeTemplateResponse>
    {
        private readonly IGenericService Service;

        public PipeTemplateValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.LaborDayPrice).GreaterThan(0).WithMessage("Labor Day cost must be defined!");
            RuleFor(x => x.EquivalentLenghPrice).GreaterThan(0).WithMessage("Pipe lenght cost must be defined!");

            RuleFor(x => x.Material).NotEqual(MaterialEnum.None).WithMessage("Material must be defined!");
            RuleFor(x => x.Diameter).NotEqual(NominalDiameterEnum.None).WithMessage("Diameter must be defined!");
            RuleFor(x => x.Class).NotEqual(PipeClassEnum.None).WithMessage("Pipe class must be defined!");

            RuleFor(x => x.Diameter).MustAsync(ReviewIfNameExist).When(x => x.Diameter!=NominalDiameterEnum.None)
                .WithMessage(x => $"Template already exist");

           

        }

        async Task<bool> ReviewIfNameExist(PipeTemplateResponse request, NominalDiameterEnum name, CancellationToken cancellationToken)
        {
            ValidatePipeTemplateRequest validate = new()
            {
               
                Class = request.Class.Name,
                Diameter = request.Diameter.Name,
                Insulation = request.Insulation,
                Material = request.Material.Name,
                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
        
    }
}
