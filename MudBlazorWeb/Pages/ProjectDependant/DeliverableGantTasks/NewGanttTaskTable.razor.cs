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
    #region Blazor
    private string GetDayStyle(bool isWeekend)
    {
        if (isWeekend)
        {
            return "background-color: #ffebee;";
        }
        return "";
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
        TimeScale.Weekly => 40,
        TimeScale.Monthly => 50,
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

    #endregion
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
    int offsetDays(DeliverableGanttTaskResponse context) => (context.StartDate!.Value.Date - Response.ProjectStart!.Value.Date).Days;
    int offsetUnits(DeliverableGanttTaskResponse context) => (int)(offsetDays(context) / scaleFactor);
    string GetLeft(DeliverableGanttTaskResponse context)
    {
        return $"{offsetUnits(context) * GetScaleWidth(SelectedScale) + 10}px";
    }
    double durationDays(DeliverableGanttTaskResponse context) => context.DurationInDays + 1;
    int durationUnits(DeliverableGanttTaskResponse context) => (int)(durationDays(context) / scaleFactor);
    string GetWidth(DeliverableGanttTaskResponse context)
    {
        return $"{Math.Max(1, durationUnits(context) * GetScaleWidth(SelectedScale) - 10)}px";
    }
}
