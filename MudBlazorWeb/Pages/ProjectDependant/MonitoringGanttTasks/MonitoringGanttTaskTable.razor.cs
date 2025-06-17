using MudBlazorWeb.Services.Enums;
using Shared.Enums.TaskStatus;
using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.DeliverableGanttTasks.Helpers;
using Shared.Models.DeliverableGanttTasks.Records;
using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.MonitoringTask.Helpers;
using Shared.Models.MonitoringTask.Records;
using Shared.Models.MonitoringTask.Responses;
using Shared.Models.Projects.Reponses;
using System.Globalization;
namespace MudBlazorWeb.Pages.ProjectDependant.MonitoringGanttTasks;
public partial class MonitoringGanttTaskTable
{
    MonitoringGanttTaskResponseList Response { get; set; } = new();
    public BudgetItemWithPurchaseOrderResponseList BudgetItemResponse { get; set; } = new();
    [Parameter]
    public ProjectResponse Project { get; set; } = null!;
    protected override async Task OnInitializedAsync()
    {
        await GetAll();

        loaded = true;
    }
    bool loaded = false;
    async Task GetAll()
    {
        await GetAllBudgetItems();
        await GetAllItems();
    }
    async Task GetAllItems()
    {
        if (Project == null) return;

        var result = await GenericService.GetAll<MonitoringGanttTaskResponseList, GetAllMonitoringGanttTask>(new GetAllMonitoringGanttTask
        {
            ProjectId = Project.Id,
        });
        if (result.Succeeded)
        {
            Response = result.Data;
            Response.UpdateSubTaskAndDependencies();
            Response.UpdateBudgetItems(BudgetItemResponse);
        }


    }
    async Task GetAllBudgetItems()
    {

        var result = await GenericService.GetAll<BudgetItemWithPurchaseOrderResponseList, BudgetItemWithPurchaseOrderGetAll>(new BudgetItemWithPurchaseOrderGetAll()
        {
            ProjectId = Project.Id,
        });
        if (result.Succeeded)
        {
            BudgetItemResponse = result.Data;

            BudgetItemResponse.OrderedItems.ForEach(x =>
            {
                x.BudgetItemGanttTasks.ForEach(y =>
                {
                    y.BudgetItem = BudgetItemResponse.OrderedItems.FirstOrDefault(z => z.Id == y.BudgetItemId);
                });
            });

        }

    }
    async Task OnChangeStatus(MonitoringGanttTaskResponse response, GanttTaskStatusEnum status)
    {
        response.DaBaseTaskStatus = status;
        ChangeStatusGanttTaskRequest request = new ChangeStatusGanttTaskRequest
        {
            Id = response.Id,
            Status = response.DaBaseTaskStatus,
            ProjectId = Project.Id
        };
        var result = await GenericService.Post(request);
        if (result.Succeeded)
        {
            
        }
    }
   
}
