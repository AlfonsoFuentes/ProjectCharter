using Shared.Models.BudgetItems.Responses;
using Shared.Models.ExpendingTools.Records;
using Shared.Models.MonitoringExpendingProjectTool.Records;
using Shared.Models.MonitoringExpendingTools.Responses;
using Shared.Models.Projects.Records;
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
            if (SelectedProject != null && selectmonth != null!)
            {
                await ShowPurchaseOrders(new ProjectMonitoringReportDto() { Id = SelectedProject.Id }, selectmonth);
            }
        }


    }
    ProjectWithPurchaseOrdersResponse SelectedProject = null!;
    MonthlyMonitoringData selectmonth = null!;
    async Task ShowPurchaseOrders(ProjectMonitoringReportDto project, MonthlyMonitoringData month)
    {
        if (SelectedProject != null && project.Id == SelectedProject.Id && selectmonth.Month == month.Month && selectmonth.Year == month.Year)
        {
            SelectedProject = null!;
            return;
        }
        selectmonth = month;
        var result = await GenericService.GetById<ProjectWithPurchaseOrdersResponse, GetProjectWithPurchaseOrderGetByIdAndDate>(new GetProjectWithPurchaseOrderGetByIdAndDate
        {
            Id = project.Id,
            Year = month.Year,
            Month = month.Month,

        });
        if (result.Succeeded)
        {
            SelectedProject = result.Data;

        }
        else
        {
            SelectedProject = null!;

        }



    }
    void CancelSelectedbudgetItem()
    {
        SelectedProject = null!;
    }
}
