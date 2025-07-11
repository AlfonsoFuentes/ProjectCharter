using MudBlazor;
using MudBlazorWeb.Pages.ProjectDependant.DeliverableGantTasks;
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
using static MudBlazorWeb.Pages.ProjectDependant.DeliverableGantTasks.NewGanttTaskTable;
using static MudBlazorWeb.Pages.ProjectDependant.ExecutionPlan.ExecutionPlanPage;
namespace MudBlazorWeb.Pages.ProjectDependant.MonitoringGanttTasks;
public partial class MonitoringGanttTaskTable
{
    MonitoringGanttTaskResponseList Response { get; set; } = new();
    public BudgetItemWithPurchaseOrderResponseList BudgetItemResponse { get; set; } = new();
    [Parameter]
    public ProjectResponse Project { get; set; } = null!;
    private List<GanttMonth> timeline = new();
    protected override async Task OnInitializedAsync()
    {
        await GetAll();

       
    }

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
            if (Response.Items.Count > 0)
            {
                GenerateTimeline();

            }
           
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
        response.TaskStatus = status;
        ChangeStatusGanttTaskRequest request = new ChangeStatusGanttTaskRequest
        {
            Id = response.Id,
            Status = response.TaskStatus,
            ProjectId = Project.Id
        };
        var result = await GenericService.Post(request);
        if (result.Succeeded)
        {
            await GetAll();
        }
    }
    async Task Edit(MonitoringGanttTaskResponse editrow)
    {
        //EditRow = editrow;
        if (editrow != null)
        {
            var parameters = new DialogParameters<MonitoringGanttTaskDialog>
            {
              
                { x => x.Response, editrow },
               

            };
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
            var dialog = await DialogService.ShowAsync<MonitoringGanttTaskDialog>("Edit current Dates", parameters, options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {

                await GetAll();
            }

        }

    }
    private (DateTime MinDate, DateTime MaxDate) GetGlobalDateRange(List<MonitoringGanttTaskResponse> items)
    {
        var startdate = items.Where(x => x.StartDate != null && x.StartDate.HasValue).Min(x => x.StartDate!.Value);
        var enddate = items.Where(x => x.EndDate != null && x.EndDate.HasValue).Max(x => x.EndDate!.Value);
        var realstartdate = items.Where(x => x.RealStartDate != null && x.RealStartDate.HasValue).Min(x => x.RealStartDate!.Value);
        var realenddate = items.Where(x => x.RealEndDate != null && x.RealEndDate.HasValue).Max(x => x.RealEndDate!.Value);

        startdate = startdate < realstartdate ? startdate : realstartdate;
        enddate = enddate > realstartdate ? enddate : realstartdate;
        return (startdate, enddate);
    }
    private void GenerateTimeline()
    {
        var result = GetGlobalDateRange(Response.Items);
        var months = new List<GanttMonth>();
        var current = new DateOnly(result.MinDate.Year, result.MinDate.Month, 1);
        var end = new DateOnly(result.MaxDate.Year, result.MaxDate.Month, 1);
        int index = 0;

        while (current <= end)
        {
            months.Add(new GanttMonth
            {
                Label = $"{current:MMM}{current:yyyy}",
                StartDate = current,
                PositionIndex = index++
            });

            current = current.AddMonths(1);
        }
        timeline = months;
        MapTasksToGantt(Response.Items, timeline);
    }
    class GanttMonth
    {
        public string Label { get; set; } = "";
        public DateOnly StartDate { get; set; }
        public int PositionIndex { get; set; }
    }
    private void MapTasksToGantt(List<MonitoringGanttTaskResponse> tasks, List<GanttMonth> timeline)
    {
        var result = new List<GanttTask>();

        foreach (var task in tasks.OrderBy(x => x.MainOrder))
        {

            DateOnly taskEnd = new DateOnly(task.EndDate!.Value.Year, task.EndDate!.Value.Month, 1);
            DateOnly taskStart = new DateOnly(task.StartDate!.Value.Year, task.StartDate!.Value.Month, 1);
            //  Aseguramos que cuente todos los meses completos entre fechas
            int durationInMonths = ((taskEnd.Year - taskStart.Year) * 12 + taskEnd.Month - taskStart.Month) + 1;
         
            int startPos = timeline.First(m => m.StartDate >= taskStart).PositionIndex;
            if (startPos == -1 || durationInMonths <= 0) continue;
            task.StartPositionIndex = startPos;
            //task.DurationInMonths = durationInMonths;

            DateOnly RealtaskEnd = new DateOnly(task.RealEndDate!.Value.Year, task.RealEndDate!.Value.Month, 1);
            DateOnly RealtaskStart = new DateOnly(task.RealStartDate!.Value.Year, task.RealStartDate!.Value.Month, 1);


            int RealdurationInMonths = ((RealtaskEnd.Year - RealtaskStart.Year) * 12 + RealtaskEnd.Month - RealtaskStart.Month) + 1;

            var realstartPos = timeline.FindIndex(m => m.StartDate >= RealtaskStart);

            task.RealStartPositionIndex = realstartPos;
            if(task.PlannedStartDate.HasValue)
            {
                task.HasPlanned = true;
                DateOnly PlannedtaskStart = new DateOnly(task.PlannedStartDate!.Value.Year, task.PlannedStartDate!.Value.Month, 1);
                task.PlannedStartPositionIndex = timeline.FindIndex(m => m.StartDate >= PlannedtaskStart);

            }


        }


    }
}
