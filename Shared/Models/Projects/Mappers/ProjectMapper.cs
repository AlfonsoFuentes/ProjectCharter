using Shared.Models.Projects.Reponses;

namespace Shared.Models.Projects.Mappers
{
    public static class ProjectMapper
    {
        public static ApproveProjectRequest ToApprove(this ProjectResponse response)
        {
            return new()
            {
                Name = response.Name,

                InitialProjectDate = response.InitialProjectDate,
                IsProductiveAsset = response.IsProductiveAsset,
                PercentageContingency = response.PercentageContingency,
                PercentageEngineering = response.PercentageEngineering,
                PercentageTaxProductive = response.PercentageTaxProductive,
                ProjectNeedType = response.ProjectNeedType,
                Status = response.Status,
                Id = response.Id,
                CostCenter = response.CostCenter,
                Focus = response.Focus,
                BudgetItems=response.BudgetItems,   

            };
        }
    }

}
