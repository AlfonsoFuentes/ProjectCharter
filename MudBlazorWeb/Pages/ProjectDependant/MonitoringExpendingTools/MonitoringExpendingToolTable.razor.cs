using Shared.Models.ExpendingTools.Records;
using Shared.Models.ExpendingTools.Responses;
using Shared.Models.MonitoringExpendingTools.Responses;
using Shared.Models.Projects.Reponses;

namespace MudBlazorWeb.Pages.ProjectDependant.MonitoringExpendingTools;
public partial class MonitoringExpendingToolTable
{
    BudgetItemMonitoringReportDtoList Response { get; set; } = new();
    [Parameter]
    public ProjectResponse Project { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await GetAll();
    }
    async Task GetAll()
    {
        if (Project == null) return;

        var result = await GenericService.GetAll<BudgetItemMonitoringReportDtoList, GetAllMonitoringExpendingTool>(new GetAllMonitoringExpendingTool
        {
            ProjectId = Project.Id,
        });
        if (result.Succeeded)
        {
            Response = result.Data;

        }


    }
}
