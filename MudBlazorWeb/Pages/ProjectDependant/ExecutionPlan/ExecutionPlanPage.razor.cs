using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.DeliverableGanttTasks.Helpers;
using Shared.Models.DeliverableGanttTasks.Records;
using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.Projects.Reponses;
using System.Globalization;
using static MudBlazorWeb.Pages.ProjectDependant.DeliverableGantTasks.NewGanttTaskTable;

namespace MudBlazorWeb.Pages.ProjectDependant.ExecutionPlan;
public partial class ExecutionPlanPage
{
    DeliverableGanttTaskResponseList Response { get; set; } = new();
    [Parameter]
    public ProjectResponse Project { get; set; } = null!;


    public BudgetItemResponseList BudgetItemResponse { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await GetAll();


    }
  
    async Task GetAll()
    {
        await GetAllBudgetItems();
        await GetAllDeliverable();
    }
    List<TaskItem> Tasks = new();
    async Task GetAllDeliverable()
    {
        if (Project == null) return;

        var result = await GenericService.GetAll<DeliverableGanttTaskResponseList, GetAllDeliverableGanttTask>(new GetAllDeliverableGanttTask
        {
            ProjectId = Project.Id,
        });
        if (result.Succeeded)
        {

            Response = result.Data;
            Response.UpdateSubTaskAndDependencies();
            Response.UpdateBudgetItems(BudgetItemResponse);

            Tasks = result.Data.OrderedItems.Select(x => new TaskItem()
            {
                BudgetAssignedUSD = x.BudgetAssignedUSD,
                EndDate = x.EndDate!.Value,

                MainOrder = x.MainOrder,
                Name = x.Name,
                StartDate = x.StartDate!.Value,
                WBS = x.WBS,
            }).ToList();
            var (minDate, maxDate) = GetGlobalDateRange(Tasks);
            timeline = GenerateTimeline(minDate, maxDate);
            ganttTasks = MapTasksToGantt(Tasks, timeline);
        
        }


    }
    async Task GetAllBudgetItems()
    {

        var result = await GenericService.GetAll<BudgetItemResponseList, BudgetItemGetAll>(new BudgetItemGetAll()
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
    public class TaskItem
    {
        public int MainOrder { get; set; }
        public string WBS { get; set; } = "";
        public string Name { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string stringStartDate => StartDate.ToString("MMM yyyy");
        public string stringEndDate => EndDate.ToString("MMM yyyy");
        public double BudgetAssignedUSD { get; set; }
        public string BudgetAssignedUSDCurrency => BudgetAssignedUSD.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
        public bool HasSubTask => false;
    }

   
    public class GanttTask
    {
        public string Name { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StartPositionIndex { get; set; }
        public int DurationInMonths { get; set; }
    }

    class GanttMonth
    {
        public string Label { get; set; } = "";
        public DateTime StartDate { get; set; }
        public int PositionIndex { get; set; }
    }

    private List<GanttMonth> timeline = new();
    private List<GanttTask> ganttTasks = new();




    private (DateTime MinDate, DateTime MaxDate) GetGlobalDateRange(IEnumerable<TaskItem> items)
    {
        var allDates = items.SelectMany(x => new[] { x.StartDate, x.EndDate });
        return (allDates.Min(), allDates.Max());
    }

    private List<GanttMonth> GenerateTimeline(DateTime startDate, DateTime endDate)
    {
        var months = new List<GanttMonth>();
        var current = new DateTime(startDate.Year, startDate.Month, 1);
        var end = new DateTime(endDate.Year, endDate.Month, 1);
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

        return months;
    }

    private List<GanttTask> MapTasksToGantt(List<TaskItem> tasks, List<GanttMonth> timeline)
    {
        var result = new List<GanttTask>();

        foreach (var task in tasks)
        {
            var taskStart = new DateTime(task.StartDate.Year, task.StartDate.Month, 1);
            var taskEnd = new DateTime(task.EndDate.Year, task.EndDate.Month, 1);

            //  Aseguramos que cuente todos los meses completos entre fechas
            int durationInMonths = ((taskEnd.Year - taskStart.Year) * 12 + taskEnd.Month - taskStart.Month) + 1;

            var startPos = timeline.FindIndex(m => m.StartDate >= taskStart);
            if (startPos == -1 || durationInMonths <= 0) continue;

            result.Add(new GanttTask
            {
                Name = task.Name,
                StartDate = taskStart,
                EndDate = taskEnd,
                StartPositionIndex = startPos,
                DurationInMonths = durationInMonths
            });
        }

        return result;
    }
}
