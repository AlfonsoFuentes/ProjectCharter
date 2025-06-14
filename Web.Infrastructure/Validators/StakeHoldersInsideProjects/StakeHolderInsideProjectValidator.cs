using Shared.Enums.StakeHolderTypes;
using Shared.Models.StakeHolderInsideProjects.Responses;
using Shared.Models.StakeHolderInsideProjects.Validators;
using Shared.Models.StakeHolders.Responses;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.StakeHoldersInsideProjects
{

    public class StakeHolderInsideProjectValidator : AbstractValidator<StakeHolderInsideProjectResponse>
    {
        private readonly IGenericService Service;

        public StakeHolderInsideProjectValidator(IGenericService service)
        {

            RuleFor(x => x.StakeHolder).NotNull().WithMessage("StakeHolder must be defined!");

            RuleFor(x => x.Role).Must(ReviewRole).WithMessage("Role must be defined!");


            RuleFor(x => x.StakeHolder).MustAsync(ReviewIfStakeHolderExist)
                .When(x => x.StakeHolder != null)
                .WithMessage(x => $"{x.StakeHolder!.Name} already exist in project");
            Service = service;
        }


        bool ReviewRole(StakeHolderInsideProjectResponse request, StakeHolderRoleEnum roleEnum)
        {
            return roleEnum != StakeHolderRoleEnum.None;
        }
        async Task<bool> ReviewIfStakeHolderExist(StakeHolderInsideProjectResponse request, StakeHolderResponse? stakeHolder, CancellationToken cancellationToken)
        {
            ValidateStakeHolderInsideProjectRequest validate = new()
            {
                OriginalStakeHolderId = request.OriginalStakeHolderId,
                Name = stakeHolder!.Name,

                ProjectId = request.ProjectId,
                StakeHolderId = request.StakeHolder!.Id,

            };
            var result = await Service.Validate(validate);
            return !result;
        }

    }
}
