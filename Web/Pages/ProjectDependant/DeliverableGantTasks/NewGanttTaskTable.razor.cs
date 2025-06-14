using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Shared.Models.DeliverableGanttTasks.Records;
using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.Projects.Reponses;
using Web.Templates;

namespace Web.Pages.ProjectDependant.DeliverableGantTasks;
public partial class NewGanttTaskTable
{
    DeliverableGanttTaskResponseList Response { get; set; } = new();
    [Parameter]
    public ProjectResponse Project { get; set; } = null!;
    DeliverableGanttTaskResponse SelectedRow = null!;
    DeliverableGanttTaskResponse? EditRow = null!;

    protected override async Task OnInitializedAsync()
    {
        await GetAll();

    }
    async Task GetAll()
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

        }


    }
    string LegendAdd => SelectedRow == null ? $"Add new deliverable to {Project.Name}" : $"Add Subtask to {SelectedRow.Name}";
    public async Task AddRow()
    {
        if (SelectedRow == null)
        {
            var parameters = new DialogParameters<DialogTemplate>
            {
                { x => x.ContentText, $"Do you really want to add new deliverable to {Project.Name}?" },
                { x => x.ButtonText, "Add" },
                { x => x.Color, Color.Warning }
            };

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = await DialogService.ShowAsync<DialogTemplate>("Question", parameters, options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                EditRow = Response.Create();
            }
        }
        else
        {
            var parameters = new DialogParameters<DialogTemplate>
            {
                { x => x.ContentText, $"Do you really want add sub task to {SelectedRow.Name}?" },
                { x => x.ButtonText, "Add" },
                { x => x.Color, Color.Warning }
            };

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = await DialogService.ShowAsync<DialogTemplate>("Question", parameters, options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                EditRow = Response.Create(SelectedRow);
            }

        }
    }
    public void AddDeliverable()
    {
        EditRow = Response.Create();



    }

    void AddSubTask()
    {
        if (SelectedRow == null) return;
        EditRow = Response.Create(SelectedRow);


    }

    async Task Delete(DeliverableGanttTaskResponse selected)
    {
        if (selected == null) return;
        var parameters = new DialogParameters<DialogTemplate>
        {
            { x => x.ContentText, $"Do you really want to delete {selected.Name}? This process cannot be undone." },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DialogTemplate>("Delete", parameters, options);
        var result = await dialog.Result;


        if (!result!.Canceled)
        {
            Response.Delete(selected);

            var resultRecalculate = await GenericService.Post(Response);
            if (resultRecalculate.Succeeded)
            {
                await GetAll();
                _snackBar.ShowSuccess(resultRecalculate.Messages);
            }
            else
            {
                _snackBar.ShowError(resultRecalculate.Messages);
            }


        }

    }

    void Cancel(DeliverableGanttTaskResponse selectedRow)
    {
        if (selectedRow == null) return;
        if (selectedRow.IsCreating)
            Response.CancelCreate(selectedRow);
        else if (backupEdit != null)
        {
            backupEdit.MapRow(selectedRow);
        }


        EditRow = null!;

        StateHasChanged();
    }
    async Task Save()
    {
        if (EditRow == null) return;
        IResult result = null!;


        result = await GenericService.Post(Response);

        if (result.Succeeded)
        {
            await GetAll();

            _snackBar.ShowSuccess(result.Messages);
        }
        else
        {
            _snackBar.ShowError(result.Messages);
        }
        EditRow = null!;



    }
    void Edit(DeliverableGanttTaskResponse selectedRow)
    {
        if (selectedRow == null) return;
        EditRow = selectedRow;
        backupEdit = selectedRow.MapRowCreational();




    }
    DeliverableGanttTaskResponse backupEdit = null!;


    void OnSelectRow(DeliverableGanttTaskResponse selectedRow)
    {
        if (EditRow != null)
        {
            return;
        }
        SelectedRow = SelectedRow == null ? selectedRow : selectedRow.Id == SelectedRow.Id ? null! : selectedRow;
        EditRow = null!;

        StateHasChanged();
    }

    string SelectedRowName => SelectedRow == null ? string.Empty : SelectedRow.Name;

    async Task MoveDown()
    {
        if (SelectedRow == null) return;
        if (Response.MoveDown(SelectedRow))
        {
            Response.Reorder();
            var result = await GenericService.Post(Response);

            if (result.Succeeded)
            {
                _snackBar.ShowSuccess(result.Messages);
            }
            else
            {
                _snackBar.ShowError(result.Messages);
            }
        }

    }

    async Task MoveUp()
    {
        if (SelectedRow == null) return;

        if (Response.MoveUp(SelectedRow))
        {
            Response.Reorder();

            var result = await GenericService.Post(Response);

            if (result.Succeeded)
            {
                _snackBar.ShowSuccess(result.Messages);
            }
            else
            {
                _snackBar.ShowError(result.Messages);
            }
        }




    }
    async Task MoveRight()
    {
        if (SelectedRow == null) return;
        if (Response.MoveRight(SelectedRow))
        {
            Response.Reorder();
            var result = await GenericService.Post(Response);
            if (result.Succeeded)
            {
                _snackBar.ShowSuccess(result.Messages);
            }
            else
            {
                _snackBar.ShowError(result.Messages);
            }

        }


    }
    async Task MoveLeft()
    {
        if (SelectedRow == null) return;
        if (Response.MoveLeft(SelectedRow))
        {
            Response.Reorder();
            var result = await GenericService.Post(Response);
            if (result.Succeeded)
            {
                _snackBar.ShowSuccess(result.Messages);
            }
            else
            {
                _snackBar.ShowError(result.Messages);
            }
        }



    }
    bool DisableButtonCanMoveDown()
    {
        if (SelectedRow == null) return true;

        return Response.DisableButtonCanMoveDown(SelectedRow);

    }
    bool DisableButtonCanMoveUp()
    {
        if (SelectedRow == null) return true;
        return Response.DisableButtonCanMoveUp(SelectedRow);
    }
    bool DisableButtonCanMoveLeft()
    {
        if (SelectedRow == null) return true;
        return Response.DisableButtonCanMoveLeft(SelectedRow);
    }
    bool DisableButtonCanMoveRight()
    {
        if (SelectedRow == null) return true;
        return Response.DisableButtonCanMoveRight(SelectedRow);
    }

    void OnChangeStartDate()
    {
        if (EditRow == null) return;
        EditRow.CalculateEndDate();
        StateHasChanged();
    }
    void OnChangeEndDate()
    {
        if (EditRow == null) return;

        EditRow.CalculateDuration();
        StateHasChanged();
    }
    void OnChangeDuration(/*string? newDuration*/)
    {
        if (EditRow == null) return;

        EditRow.CalculateEndDate();

        StateHasChanged();
    }
    void OnChangeDependencies(string newDependencies)
    {
        if (EditRow == null) return;

        var currenDependencies = newDependencies;

        var lasdependency = currenDependencies.Last();
        if (lasdependency == ',')
        {
            EditRow.DependencyList = currenDependencies;
            return;
        }

        var resultMessages = Response.SetDependeyList(EditRow, newDependencies);
        if (!string.IsNullOrEmpty(resultMessages))
        {
            _snackBar.ShowError(resultMessages);
            EditRow.DependencyList = string.Empty;
        }

        StateHasChanged();
    }

    private bool Validated { get; set; } = true;
    // Método asincrónico para realizar la validación
    public async Task ValidateAsync()
    {
        Validated = true;// _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }


    FluentValidationValidator _fluentValidationValidator = null!;

}
