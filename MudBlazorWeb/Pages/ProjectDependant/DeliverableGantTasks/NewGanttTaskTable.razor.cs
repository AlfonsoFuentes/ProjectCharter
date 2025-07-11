using MudBlazor;
using MudBlazorWeb.Services.Enums;
using MudBlazorWeb.Templates;
using Polly;
using Shared.Models.BudgetItems.Mappers;
using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.DeliverableGanttTasks.Helpers;
using Shared.Models.DeliverableGanttTasks.Records;
using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.Projects.Reponses;
using System.Globalization;
using static MudBlazorWeb.Pages.ProjectDependant.ExecutionPlan.ExecutionPlanPage;

namespace MudBlazorWeb.Pages.ProjectDependant.DeliverableGantTasks;
public partial class NewGanttTaskTable
{
    DeliverableGanttTaskResponseList Response { get; set; } = new();
    [Parameter]
    public ProjectResponse Project { get; set; } = null!;
    DeliverableGanttTaskResponse SelectedRow = null!;

    public BudgetItemResponseList BudgetItemResponse { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await GetAll();

        loaded = true;
    }
    bool loaded = false;
    async Task GetAll()
    {
        await GetAllBudgetItems();
        await GetAllDeliverable();
    }
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
            if (Response.Items.Count > 0)
            {
                GenerateTimeline();

            }

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

    string LegendAdd => SelectedRow == null ? $"Add new deliverable to {Project.Name}" : $"Add Subtask to {SelectedRow.Name}";
    public async Task AddRow()
    {
        DeliverableGanttTaskResponse? EditRow = null!;
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
        if (EditRow != null)
        {
            var parameters = new DialogParameters<GantTaskDialog>
            {
                { x => x.Model, EditRow },
                { x => x.Response, Response },
                { x => x.BudgetItemResponse, BudgetItemResponse },

            };
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge };
            var dialog = await DialogService.ShowAsync<GantTaskDialog>(EditRow.IsDeliverable ? "Add Deliverable" : "Add Task", parameters, options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {

                await GetAll();
            }
        }
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


    async Task Edit(DeliverableGanttTaskResponse? editrow)
    {
        //EditRow = editrow;
        if (editrow != null)
        {
            var parameters = new DialogParameters<GantTaskDialog>
            {
                { x => x.Model, editrow },
                { x => x.Response, Response },
                { x => x.BudgetItemResponse, BudgetItemResponse },

            };
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge };
            var dialog = await DialogService.ShowAsync<GantTaskDialog>(editrow.IsDeliverable ? "Edit Deliverable" : "Edit Task", parameters, options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {

                await GetAll();
            }

        }

    }

    DeliverableGanttTaskResponse EditLocalRow = null!;
    DeliverableGanttTaskResponse backupEdit = null!;
    void OnEditLocalRow(DeliverableGanttTaskResponse selectedRow)
    {

        if (selectedRow.HasSubTask) return;
        SelectedRow = null!;
        EditLocalRow = EditLocalRow == null! ? selectedRow : selectedRow.Id == EditLocalRow.Id ? null! : selectedRow;
        if (EditLocalRow != null)
        {
            backupEdit = EditLocalRow.MapRowCreational();

        }
    }
    void ChangeStartDate()
    {
        if (EditLocalRow == null) return;
        EditLocalRow.SetStartDate = true;
    }
    void ChangeEndDate(DateTime? enddate)
    {
        if (EditLocalRow == null) return;
        EditLocalRow.CalculateDurationWithEndDate(enddate);

    }
    async Task Save()
    {
        var result = await GenericService.Post(Response);
        if (result.Succeeded)
        {
            _snackBar.ShowSuccess(result.Message);

            await GetAll();
            EditLocalRow = null!;
        }
        else
        {
            _snackBar.ShowError(result.Messages);



        }
    }
    void CancelEditLocalRow(DeliverableGanttTaskResponse selectedRow)
    {
        if (backupEdit != null)
        {
            backupEdit.MapRow(selectedRow);
        }
        EditLocalRow = null!;
    }
    void OnSelectRow(DeliverableGanttTaskResponse selectedRow)
    {

        SelectedRow = SelectedRow == null ? selectedRow : selectedRow.Id == SelectedRow.Id ? null! : selectedRow;


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

    private void ValidateAsync()
    {

    }
    private List<GanttMonth> timeline = new();

    class GanttMonth
    {
        public string Label { get; set; } = "";
        public DateTime StartDate { get; set; }
        public int PositionIndex { get; set; }
    }
    private (DateTime MinDate, DateTime MaxDate) GetGlobalDateRange(IEnumerable<DeliverableGanttTaskResponse> items)
    {
        var allDates = items.SelectMany(x => new[] { x.StartDate!.Value, x.EndDate!.Value });
        return (allDates.Min(), allDates.Max());
    }
    private void GenerateTimeline()
    {
        var result = GetGlobalDateRange(Response.Items);
        var months = new List<GanttMonth>();
        var current = new DateTime(result.MinDate.Year, result.MinDate.Month, 1);
        var end = new DateTime(result.MaxDate.Year, result.MaxDate.Month, 1);
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
    private void MapTasksToGantt(List<DeliverableGanttTaskResponse> tasks, List<GanttMonth> timeline)
    {
        var result = new List<GanttTask>();

        foreach (var task in tasks)
        {
            var taskStart = new DateTime(task.StartDate!.Value.Year, task.StartDate!.Value.Month, 1);
            var taskEnd = new DateTime(task.EndDate!.Value.Year, task.EndDate!.Value.Month, 1);

            //  Aseguramos que cuente todos los meses completos entre fechas
            int durationInMonths = ((taskEnd.Year - taskStart.Year) * 12 + taskEnd.Month - taskStart.Month) + 1;

            var startPos = timeline.FindIndex(m => m.StartDate >= taskStart);
            if (startPos == -1 || durationInMonths <= 0) continue;
            task.StartPositionIndex = startPos;
            task.DurationInMonths = durationInMonths;

        }


    }
}
