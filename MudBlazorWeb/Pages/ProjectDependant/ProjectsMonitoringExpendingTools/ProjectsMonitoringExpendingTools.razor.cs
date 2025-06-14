using Shared.Models.ExpendingTools.Records;
using Shared.Models.MonitoringExpendingProjectTool.Records;
using Shared.Models.MonitoringExpendingTools.Responses;
using Shared.Models.Projects.Reponses;

namespace MudBlazorWeb.Pages.ProjectDependant.ProjectsMonitoringExpendingTools;
public partial class ProjectsMonitoringExpendingTools
{
    ProjectMonitoringReportDtoList Response { get; set; } = new();


    protected override async Task OnInitializedAsync()
    {
        await GetAll();
    }
    async Task GetAll()
    {


        var result = await GenericService.GetAll<ProjectMonitoringReportDtoList, GetAllMonitoringExpendingProjectTool>(new GetAllMonitoringExpendingProjectTool());
        if (result.Succeeded)
        {
            Response = result.Data;

        }


    }
}
