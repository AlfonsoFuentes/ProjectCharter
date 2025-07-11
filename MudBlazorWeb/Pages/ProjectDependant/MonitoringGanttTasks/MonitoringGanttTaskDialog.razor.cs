using Blazored.FluentValidation;
using MudBlazor;
using Shared.Enums.TaskStatus;
using Shared.Models.DeliverableGanttTasks.Records;
using Shared.Models.MonitoringTask.Request;
using Shared.Models.MonitoringTask.Responses;

namespace MudBlazorWeb.Pages.ProjectDependant.MonitoringGanttTasks;
public partial class MonitoringGanttTaskDialog
{

    UpdateMonitoringGanttTaskRequest Model { get; set; } = new();
    [Parameter]
    public MonitoringGanttTaskResponse Response { get; set; } = new();
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; }
    // Método asincrónico para realizar la validación
    public async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }
    FluentValidationValidator _fluentValidationValidator = null!;

    protected override void OnInitialized()
    {
        Model = new()
        {
            Name = Response.Name,
            RealEndDate = Response.RealEndDate,
            RealStartDate = Response.RealStartDate,
            RealDurationInDays = Response.RealDurationInDays,
            RealDurationInUnit = Response.RealDurationInUnit,
            RealDurationUnit = Response.RealDurationUnit,
            GanttTaskId = Response.Id,
            TaskStatus = Response.TaskStatus,

        };
    }
    private async Task Submit()
    {
        var result = await GenericService.Post(Model);
        if (result.Succeeded)
        {
            _snackBar.ShowSuccess(result.Message);
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            _snackBar.ShowError(result.Messages);

            MudDialog.Close(DialogResult.Ok(false));

        }

    }
    void Cancel()
    {

        MudDialog.Close(DialogResult.Ok(false));

    }
    void OnChangeStartDate()
    {
        if (Model == null) return;
        Model.SetStartDate = true;
        StateHasChanged();
    }
    void ChangeEndDate(DateTime? enddate)
    {
        if (Model == null) return;
        Model.RealEndDate = enddate;
        Model.CalculateDurationWithEndDate(enddate);

    }
    void OnChangeDuration(/*string? newDuration*/)
    {
        if (Model == null) return;



        StateHasChanged();
    }
    async Task OnChangeStatus(UpdateMonitoringGanttTaskRequest response, GanttTaskStatusEnum status)
    {
        response.TaskStatus = status;
        await ValidateAsync();
    }
}
