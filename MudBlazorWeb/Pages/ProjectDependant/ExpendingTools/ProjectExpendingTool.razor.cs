using Shared.Models.BudgetItems.Responses;
using Shared.Models.DeliverableGanttTasks.Helpers;
using Shared.Models.DeliverableGanttTasks.Records;
using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.ExpendingTools.Records;
using Shared.Models.ExpendingTools.Responses;
using Shared.Models.Projects.Reponses;

namespace MudBlazorWeb.Pages.ProjectDependant.ExpendingTools;
public partial class ProjectExpendingTool
{
    BudgetItemReportDtoList Response { get; set; } = new();
    [Parameter]
    public ProjectResponse Project { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await GetAll(); 
    }
    async Task GetAll()
    {
        if (Project == null) return;

        var result = await GenericService.GetAll<BudgetItemReportDtoList, GetAllExpendingTool>(new GetAllExpendingTool
        {
            ProjectId = Project.Id,
        });
        if (result.Succeeded)
        {
            Response = result.Data;
         
        }


    }
}
