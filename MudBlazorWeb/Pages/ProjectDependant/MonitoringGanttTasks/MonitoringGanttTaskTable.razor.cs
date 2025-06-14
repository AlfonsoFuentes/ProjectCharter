using MudBlazorWeb.Services.Enums;
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
    private string GetScaleLabel(DateTime date, TimeScale scale)
    {
        return scale switch
        {
            TimeScale.Daily => $"{date.Day}-{date.Month}",
            TimeScale.Weekly => $"W{GetIso8601WeekOfYear(date)}",
            TimeScale.Monthly => $"{date.ToString("MMM").ToUpper()}-{date.Year}",
            TimeScale.Quarterly => $"Q{GetQuarter(date)}-{date.Year}",
            TimeScale.SemiAnnually => $"S{(date.Month <= 6 ? "1" : "2")}-{date.Year}",
            TimeScale.Yearly => date.Year.ToString(),
            _ => date.Day.ToString()
        };
    }

    private DateTime GetNextDate(DateTime date, TimeScale scale)
    {
        return scale switch
        {
            TimeScale.Daily => date.AddDays(1),
            TimeScale.Weekly => date.AddDays(7),
            TimeScale.Monthly => new DateTime(date.Year, date.Month, 1).AddMonths(1),
            TimeScale.Quarterly => new DateTime(date.Year, ((date.Month - 1) / 3 * 3) + 1, 1).AddMonths(3),
            TimeScale.SemiAnnually => new DateTime(date.Year, (date.Month <= 6 ? 7 : 1), 1),
            TimeScale.Yearly => new DateTime(date.Year, 1, 1).AddYears(1),
            _ => date.AddDays(1)
        };
    }

    private int GetScaleWidth(TimeScale scale) => scale switch
    {
        TimeScale.Daily => 30,
        TimeScale.Weekly => 50,
        TimeScale.Monthly => 60,
        TimeScale.Quarterly => 90,
        TimeScale.SemiAnnually => 120,
        TimeScale.Yearly => 150,
        _ => 30
    };

    private int GetQuarter(DateTime date) => (date.Month + 2) / 3;

    private int GetIso8601WeekOfYear(DateTime date)
    {
        var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
        date = date.AddDays(day - DayOfWeek.Monday == -1 ? 6 : day - DayOfWeek.Monday); // Asegura lunes como inicio de semana
        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }
    private string GetScaleUnitStyle(DateTime date, TimeScale scale)
    {
        var isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

        string style = $"width: {GetScaleWidth(scale)}px;";

        if (scale == TimeScale.Daily && isWeekend)
        {
            style += " background-color: #ffebee;";
        }

        return style;
    }


    double scaleFactor => SelectedScale switch
    {
        TimeScale.Daily => 1,
        TimeScale.Weekly => 7,
        TimeScale.Monthly => 30,
        TimeScale.Quarterly => 90,
        TimeScale.SemiAnnually => 180,
        TimeScale.Yearly => 365,
        _ => 1
    };
    int offsetDays(MonitoringGanttTaskResponse context) => (context.StartDate!.Value.Date - Response.ProjectStart!.Value.Date).Days;
    int offsetPlannedDays(MonitoringGanttTaskResponse context) => context.PlannedStartDate == null ? 0 : (context.PlannedStartDate!.Value.Date - Response.ProjectStart!.Value.Date).Days;
    int offsetUnits(MonitoringGanttTaskResponse context) => (int)(offsetDays(context) / scaleFactor);
    int offsetPlannedUnits(MonitoringGanttTaskResponse context) => (int)(offsetPlannedDays(context) / scaleFactor);
    string GetLeft(MonitoringGanttTaskResponse context)
    {
        return $"{offsetUnits(context) * GetScaleWidth(SelectedScale) + 10}px";
    }
    string GetPlannedLeft(MonitoringGanttTaskResponse context)
    {
        return $"{offsetPlannedUnits(context) * GetScaleWidth(SelectedScale) + 10}px";
    }
    double durationDays(MonitoringGanttTaskResponse context) => context.DurationInDays + 1;
    double durationPlannedDays(MonitoringGanttTaskResponse context) => context.DurationPlannedInDays + 1;
    int durationUnits(MonitoringGanttTaskResponse context) => (int)(durationDays(context) / scaleFactor);
    string GetWidth(MonitoringGanttTaskResponse context)
    {
        return $"{Math.Max(1, durationUnits(context) * GetScaleWidth(SelectedScale) - 10)}px";
    }
    string GetPlannedWidth(MonitoringGanttTaskResponse context)
    {
        return $"{Math.Max(1, durationPlannedUnits(context) * GetScaleWidth(SelectedScale) - 10)}px";
    }
    int durationPlannedUnits(MonitoringGanttTaskResponse context) => (int)(durationPlannedDays(context) / scaleFactor);
}
