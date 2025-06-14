using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Shared.Models.BudgetItems.BasicEngineeringItems;
using Shared.Models.BudgetItems.Mappers;
using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.Communications.Responses;
using Shared.Models.DeliverableGanttTasks.Helpers;
using Shared.Models.DeliverableGanttTasks.Responses;
using System.Threading.Tasks;

namespace MudBlazorWeb.Pages.ProjectDependant.DeliverableGantTasks;
public partial class GantTaskDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; }
    // Método asincrónico para realizar la validación
    public async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }
    DeliverableGanttTaskResponse backupEdit = null!;

    public List<DeliverableGanttTaskResponse> GanttTaskItems { get; set; } = new();
    [Parameter]
    public BudgetItemResponseList BudgetItemResponse { get; set; } = new();


    public List<BudgetItemResponse> SelectedBudgetItems { get; set; } = new();
    public List<BudgetItemResponse> NonSelectedBudgetItems { get; set; } = new();
    [Parameter]
    public DeliverableGanttTaskResponse Model { get; set; } = new();
    [Parameter]
    public DeliverableGanttTaskResponseList Response { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        if (!Model.IsCreating)
        {
            backupEdit = Model.MapRowCreational();
        }
        RecreateBudgetItems();
        RecreateGanttTaskItems();
        if (Model.IsDeliverable)
        {
            await this.MudDialog.SetOptionsAsync(new DialogOptions() { MaxWidth = MaxWidth.Small });
        }


        StateHasChanged();

    }
    void RecreateGanttTaskItems()
    {
        GanttTaskItems = new();
        bool AddnewItem = false;
        if (Model.HasSubTask) return;

        foreach (var row in Response.Items)
        {
            if (Model.NewDependencies.Any(x => x.DependencyTaskId == row.Id))
            {
                GanttTaskItems.Add(row);

            }
            else if (Response.IsValidNewDependency(Model, row) == string.Empty)
            {
                AddnewItem = true;
                GanttTaskItems.Add(row);
            }

        }
        if (AddnewItem)
        {
            if (Model.NewDependencies.Any(x => x.DependencyTaskId == Guid.Empty)) return;
            Model.NewDependencies.Add(new()
            {
                Order = Model.LastDependencyOrder + 1,
            });
        }
    }

    void RecreateBudgetItems()
    {
        if (Model == null) return;
        bool AddnewBudgetItem = false;
        SelectedBudgetItems = new();
        NonSelectedBudgetItems = new();

        foreach (var row in BudgetItemResponse.OrderedItems)
        {
            if (Model.BudgetItemGanttTasks.Any(x => x.BudgetItemId == row.Id))
            {
                SelectedBudgetItems.Add(row);
                if(row.HasSubItems)
                {
                    NonSelectedBudgetItems.Add(row);
                }

            }
            else if (row.IsAvailableToAssignedToTask)
            {
                AddnewBudgetItem = true;
                SelectedBudgetItems.Add(row);
                NonSelectedBudgetItems.Add(row);

            }
        }

        if (AddnewBudgetItem)
        {
            if (Model.BudgetItemGanttTasks.Any(x => x.BudgetItemId == Guid.Empty)) return;
            Model.BudgetItemGanttTasks.Add(new()
            {
                Order = Model.LastBudgetItemOrder + 1,
            });
        }

    }


    FluentValidationValidator _fluentValidationValidator = null!;


    private async Task Submit()
    {
        var result = await GenericService.Post(Response);
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
        if (Model == null) return;
        if (Model.IsCreating)
            Response.CancelCreate(Model);
        else if (backupEdit != null)
        {
            backupEdit.MapRow(Model);
        }
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
        Model.CalculateDurationWithEndDate(enddate);

    }
    void OnChangeDuration(/*string? newDuration*/)
    {
        if (Model == null) return;



        StateHasChanged();
    }
    void OnChangeDependencies(string? dendependencies)
    {

        if (Model == null) return;

        var currenDependencies = dendependencies;
        if (string.IsNullOrEmpty(currenDependencies))
        {
            Model.DependencyList = string.Empty;
            return;
        }
        var lasdependency = currenDependencies!.Last();
        if (lasdependency == ',')
        {
            Model.DependencyList = currenDependencies;
            return;
        }

        var resultMessages = Response.SetDependeyList(Model, currenDependencies);
        if (!string.IsNullOrEmpty(resultMessages))
        {
            _snackBar.ShowError(resultMessages);
            Model.DependencyList = string.Empty;
        }

        StateHasChanged();
    }
}
