using Shared.Enums.CostCenter;
using Shared.Enums.Focuses;
using Shared.Enums.ProjectNeedTypes;
using Shared.Models.Projects.Request;
using Shared.Models.Projects.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Projects
{
    public class ApproveProjectValidator : AbstractValidator<ApproveProjectRequest>
    {
        private readonly IGenericService Service;

        public ApproveProjectValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            RuleFor(x => x.InitialProjectDate).NotNull().WithMessage("Initial project date must be defined!");
            RuleFor(x => x.ProjectNeedType).NotEqual(ProjectNeedTypeEnum.None).WithMessage("Type must be defined!");
            RuleFor(x => x.CostCenter).NotEqual(CostCenterEnum.None).WithMessage("Cost Center must be defined!");
            RuleFor(x => x.Focus).NotEqual(FocusEnum.None).WithMessage("Focus must be defined!");

            RuleFor(x => x.ProjectNumber).Matches("^[0-9]*$").WithMessage("Project Number must be number!");
            RuleFor(x => x.ProjectNumber).NotNull().WithMessage("Project Number must be defined");
            RuleFor(x => x.ProjectNumber).NotEmpty().WithMessage("Project Number must be defined");
            RuleFor(x => x.ProjectNumber).Length(5).WithMessage("Project Number must have 5 digits");

            RuleFor(x => x.ProjectNumber).MustAsync(ReviewIfProjectNumberExist)
                 .When(x => !string.IsNullOrEmpty(x.ProjectNumber))
                 .WithMessage(x => $"{x.ProjectNumber} already exist");
           
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");
            RuleFor(x => x.PercentageContingency).GreaterThan(0).WithMessage("%Contingency must be defined!");
            RuleFor(x => x.PercentageEngineering).GreaterThan(0).WithMessage("%Engineering must be defined!");
            RuleFor(x => x.BudgetItems)
                .NotEqual(0)
                   .When(x => !string.IsNullOrEmpty(x.ProjectNumber))
                   .WithMessage("Budget items must be defined!");
            RuleFor(x => x.PercentageTaxProductive).GreaterThan(0).When(x => x.IsProductiveAsset == false).WithMessage("%Tax (VAT) must be defined!");
        }


        async Task<bool> ReviewIfNameExist(ApproveProjectRequest request, string name, CancellationToken cancellationToken)
        {
            ValidateProjectRequest validate = new()
            {
                Name = name,

                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
        async Task<bool> ReviewIfProjectNumberExist(ApproveProjectRequest request, string name, CancellationToken cancellationToken)
        {
            ValidateProjectNumberRequest validate = new()
            {
                ProjectNumber = request.ProjectNumber,

                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
