using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Responses;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.BudgetItems.Pipings
{
   
    public class PipeValidator : AbstractValidator<PipeResponse>
    {
        private readonly IGenericService Service;

        public PipeValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
          //  RuleFor(x => x.BudgetUSD).GreaterThan(0).When(x => !x.IsExisting).WithMessage("Budget must be defined!");
          //  RuleFor(x => x.MaterialQuantity).GreaterThan(0).When(x => x.ShowDetails).WithMessage("Equivalent lenght must be defined!");
          //  RuleFor(x => x.LaborQuantity).GreaterThan(0).When(x => x.ShowDetails).WithMessage("#Labor days must be defined!");

          //  RuleFor(x => x.LaborDayPrice).GreaterThan(0).When(x => x.ShowDetails).WithMessage("Unitary cost for Labor/day must be defined!");
          //  RuleFor(x => x.EquivalentLenghPrice).GreaterThan(0).When(x => x.ShowDetails).WithMessage("Unitary cost for equivalent lenght/mt must be defined!");


          //  RuleFor(x => x.Tag).NotEmpty().When(x => x.ShowDetails)
          //    .WithMessage("Tag Letter must be defined!");

          

          //  RuleFor(x => x.FluidCode).NotNull().When(x => x.ShowDetails)
          //.WithMessage("Fluid must be defined!");

          //  RuleFor(x => x.Template.Material).NotEqual(MaterialEnum.None).When(x => x.ShowDetails)
          //.WithMessage("Internal Material must be defined!");

          //  RuleFor(x => x.Template.Diameter).NotEqual(NominalDiameterEnum.None).When(x => x.ShowDetails)
          //.WithMessage("Internal Material must be defined!");

          //  RuleFor(x => x.Template.Class).NotEqual(PipeClassEnum.None).When(x => x.ShowDetails)
          //.WithMessage("Pipe Class must be defined!");


           
          //  RuleFor(x => x.TagNumber).NotEmpty().When(x => x.ShowDetails)
          //      .WithMessage("Tag Number must be defined!");

          //  RuleFor(x => x.Tag).NotEmpty().When(x => x.ShowDetails)
          //      .MustAsync(ReviewIfTagExist)
          //     .WithMessage(x => $"{x.Tag} already exist");

            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

            

        }

        async Task<bool> ReviewIfNameExist(PipeResponse request, string name, CancellationToken cancellationToken)
        {
            ValidatePipeRequest validate = new()
            {
                Name = name,
                ProjectId = request.ProjectId,
                Id = request.Id,
              

            };
            var result = await Service.Validate(validate);
            return !result;
        }
        //async Task<bool> ReviewIfTagExist(PipeResponse request, string name, CancellationToken cancellationToken)
        //{
        //    ValidatePipeTagRequest validate = new()
        //    {
               
        //        Id = request.Id,
        //        Tag = request.Tag,

        //        ProjectId = request.ProjectId,

        //    };
        //    var result = await Service.Validate(validate);
        //    return !result;
        //}
        
    }
}
