using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;
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
            if (SelectedbudgetItem != null && selectmonth != null!)
            {
                await ShowPurchaseOrders(new() { Id = SelectedbudgetItem.Id }, selectmonth);
            }

        }

    }
    BudgetItemWithPurchaseOrdersResponse SelectedbudgetItem = null!;
    MonthlyMonitoringData selectmonth = null!;
    async Task ShowPurchaseOrders(BudgetItemMonitoringReportDto budgetItem, MonthlyMonitoringData month)
    {
        if(SelectedbudgetItem != null && budgetItem.Id == SelectedbudgetItem.Id && selectmonth.Month == month.Month && selectmonth.Year == month.Year)
        {
            SelectedbudgetItem = null!;
            return;
        }

        selectmonth = month;
        var result = await GenericService.GetById<BudgetItemWithPurchaseOrdersResponse, BudgetItemWithPurchaseOrderGetByIdAndDate>(new BudgetItemWithPurchaseOrderGetByIdAndDate
        {
            Id = budgetItem.Id,
            Year = month.Year,
            Month = month.Month,
            ProjectId = Project.Id
        });
        if (result.Succeeded)
        {
            SelectedbudgetItem = result.Data;

        }
        else
        {
            SelectedbudgetItem = null!;

        }



    }
    void CancelSelectedbudgetItem()
    {
        SelectedbudgetItem = null!;
    }
}
