using MudBlazor;
using MudBlazorWeb.Pages.ProjectDependant.MonitoringGanttTasks;
using MudBlazorWeb.Pages.PurchaseOrders.Dialogs;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.MonitoringTask.Responses;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.Warnings.Records;
using Shared.Models.Warnings.Responses;

namespace MudBlazorWeb.Pages.ProjectDependant.Warnings;
public partial class WarningsGetAll
{
    WarningResponseList Response = new();

    async Task GetAll()
    {

        var result = await GenericService.GetAll<WarningResponseList, WarningGetAll>(new WarningGetAll());
        if (result.Succeeded)
        {
            Response = result.Data;
        }
    }
    protected override async Task OnInitializedAsync()
    {
        await GetAll();

    }
}
