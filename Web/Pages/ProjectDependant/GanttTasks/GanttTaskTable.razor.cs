namespace Web.Pages.ProjectDependant.GanttTasks;
public partial class GanttTaskTable
{
    //[Parameter]
    //public ProjectResponse Project { get; set; } = null!;

    //[Parameter]
    //public DeliverableWithGanttTaskResponseList Response { get; set; } = null!;
    //protected override async Task OnInitializedAsync()
    //{
    //    if (Project == null) return;
     
    //    await GetAll();
    //}
    //async Task GetAll()
    //{

    //    var result = await GenericService.GetAll<DeliverableWithGanttTaskResponseListToUpdate, GanttTaskGetAll>(new GanttTaskGetAll
    //    {
    //        ProjectId = Project.Id,
    //    });

    //    if (result.Succeeded)
    //    {

    //        Response = result.Data.ToReponse();

    //        StateHasChanged();

    //    }

    //}
    //private void OnClickDeliverable(DeliverableWithGanttTaskResponse deliverable)
    //{
    //    SelectedDeliverable = deliverable;
    //}

    //public async Task AddNewDeliverable()
    //{

    //    var parameters = new DialogParameters<DeliverableDialog>
    //    {
    //        { x => x.Model, new(){ProjectId=Project.Id } },
    //    };

    //    var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

    //    var dialog = await DialogService.ShowAsync<DeliverableDialog>("Deliverable", parameters, options);
    //    var result = await dialog.Result;
    //    if (result != null)
    //    {
    //        await GetAll();
    //        StateHasChanged();
    //    }
    //}

    //GanttTaskResponse CreateRow = null!;
    //GanttTaskResponse EditRow = null!;
    //GanttTaskResponse SelectedRow = null!;
    //// Propiedad para deshabilitar el botón "Up"
    //private bool DisableUpButton => SelectedDeliverable == null ? true : SelectedRow == null || !SelectedDeliverable.CanMoveUp(SelectedRow) || CreateRow != null || EditRow != null;
    //DeliverableWithGanttTaskResponse? SelectedDeliverable = null!;
    //private bool DisableAddButton => SelectedDeliverable == null ? true : CreateRow != null || EditRow != null;
    //string CurrentDeliverableName => SelectedDeliverable == null ? string.Empty : SelectedDeliverable.Name;
    //string CurrentRowName
    //{
    //    get
    //    {
    //        if (EditRow != null)
    //        {
    //            return TruncateService.Truncate(EditRow.Name, 30);
    //        }

    //        if (SelectedRow != null)
    //        {
    //            return TruncateService.Truncate(SelectedRow.Name, 30);
    //        }
    //        if (SelectedDeliverable != null)
    //        {
    //            return TruncateService.Truncate(SelectedDeliverable.Name, 30);
    //        }


    //        return string.Empty;
    //    }
    //}
    //// Propiedad para deshabilitar el botón "Down"
    //bool DisableDownButton => SelectedDeliverable == null ? true : SelectedRow == null || !SelectedDeliverable.CanMoveDown(SelectedRow) || CreateRow != null || EditRow != null;
    //// Propiedad para deshabilitar el botón "Left"
    //private bool DisableLeftButton => SelectedDeliverable == null ? true : SelectedRow == null || SelectedDeliverable.FindParent(SelectedRow) == null || CreateRow != null || EditRow != null;
    //// Propiedad para deshabilitar el botón "Right"
    //private bool DisableRightButton => SelectedDeliverable == null ? true : SelectedRow == null || !SelectedDeliverable.CanMoveRight(SelectedRow) || CreateRow != null || EditRow != null;



    //public void AddNew()
    //{
    //    if (SelectedDeliverable == null) return;
    //    SelectedDeliverable.IsExpanded = true;

    //    var newDeliverable = SelectedDeliverable.AddGanttTaskResponse(SelectedDeliverable.DeliverableId, SelectedRow, Project.InitialProjectDate);
    //    CreateRow = newDeliverable;

    //}

    //void CancelEdit(GanttTaskResponse row)
    //{
    //    if (SelectedDeliverable == null) return;
    //    if (CreateRow == row)
    //    {
    //        SelectedDeliverable.RemoveGanttTaskResponse(row);

    //    }
    //    row.IsEditing = false;
    //    SelectedRow = CreateRow == row ? null! : EditRow == row ? row : null!;
    //    CreateRow = null!;
    //    EditRow = null!;

    //}
    //DeliverableWithGanttTaskResponse? FindSelectedDeliverable(GanttTaskResponse row)
    //{
    //    if (Response?.Deliverables == null)
    //    {
    //        return null;
    //    }

    //    // Itera sobre cada deliverable para encontrar uno cuyo FlatOrderedItems contenga el elemento buscado
    //    foreach (var deliverable in Response.Deliverables)
    //    {
    //        // Verifica que FlatOrderedItems no sea nulo antes de usar Contains
    //        if (deliverable.FlatOrderedItems != null && deliverable.FlatOrderedItems.Any(x => x.Id == row.Id))
    //        {
    //            return deliverable;
    //        }
    //    }

    //    // Si no se encuentra ningún deliverable que contenga el elemento, retorna null
    //    return null;

    //}
    //private void RowClick(GanttTaskResponse row)
    //{
    //    if (row.IsEditing) return;
    //    SelectedDeliverable = FindSelectedDeliverable(row)!;
    //    SelectedRow = SelectedRow == null ? row : SelectedRow == row ? null! : row;
    //    CreateRow = null!;
    //    EditRow = null!;

    //}
    //private void RowEdit(GanttTaskResponse row)
    //{
    //    SelectedDeliverable = FindSelectedDeliverable(row);
    //    if (SelectedDeliverable == null) return;
    //    SelectedDeliverable.FlatOrderedItems.ForEach(x => { x.IsEditing = false; });

    //    EditRow = row;
    //    EditRow.IsEditing = true;

    //    SelectedRow = null!;

    //    CreateRow = null!;

    //}
    //public async Task Delete(GanttTaskResponse model)
    //{
    //    SelectedDeliverable = FindSelectedDeliverable(model);
    //    if (SelectedDeliverable == null) return;
    //    if (model == null) return;
    //    var parameters = new DialogParameters<DialogTemplate>
    //    {
    //        { x => x.ContentText, $"Do you really want to delete {model.Name}? This process cannot be undone." },
    //        { x => x.ButtonText, "Delete" },
    //        { x => x.Color, Color.Error }
    //    };

    //    var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

    //    var dialog = await DialogService.ShowAsync<DialogTemplate>("Delete", parameters, options);
    //    var result = await dialog.Result;


    //    if (!result!.Canceled)
    //    {
    //        SelectedRow = null!;
    //        SelectedDeliverable.RemoveGanttTaskResponse(model);
    //        var request = new DeleteGanttTaskRequest
    //        {
    //            Id = model.Id,
    //            Name = model.Name,
    //            ProjectId = Response.ProjectId,
    //        };


    //        var resultDelete = await GenericService.Post(request);
    //        if (resultDelete.Succeeded)
    //        {
    //            await GetAll();
    //            _snackBar.ShowSuccess(resultDelete.Messages);


    //        }
    //        else
    //        {
    //            _snackBar.ShowError(resultDelete.Messages);
    //        }
    //    }

    //}

    //async Task Up()
    //{
    //    if (SelectedDeliverable == null) return;
    //    if (SelectedRow == null) return;
    //    SelectedDeliverable.MoveUp(SelectedRow);
    //    SelectedDeliverable.Calculate();
    //    StateHasChanged();
    //    await UpdateResponseAsync();

    //}
    //async Task Down()
    //{
    //    if (SelectedDeliverable == null) return;
    //    if (SelectedRow == null) return;
    //    SelectedDeliverable.MoveDown(SelectedRow);
    //    SelectedDeliverable.Calculate();
    //    StateHasChanged();
    //    await UpdateResponseAsync();

    //}
    //async Task Right()
    //{
    //    if (SelectedDeliverable == null) return;
    //    if (SelectedRow == null) return;
    //    SelectedDeliverable.MoveRight(SelectedRow);
    //    SelectedDeliverable.Calculate();
    //    StateHasChanged();
    //    await UpdateResponseAsync();

    //}
    //async Task Left()
    //{
    //    if (SelectedDeliverable == null) return;
    //    if (SelectedRow == null) return;
    //    SelectedDeliverable.MoveLeft(SelectedRow);
    //    SelectedDeliverable.Calculate();
    //    StateHasChanged();
    //    await UpdateResponseAsync();

    //}
    //private async Task<bool> UpdateResponseAsync()
    //{

    //    var result = await GenericService.Update(Response.ToUpdate());

    //    if (result.Succeeded)
    //    {
    //        await GetAll();

    //        EditRow = null!;
    //        _snackBar.ShowSuccess(result.Messages);
    //        return true;
    //    }

    //    _snackBar.ShowError(result.Messages);
    //    return false;
    //}
    //async Task Save(GanttTaskResponse row)
    //{
    //    if (row.IsEditing)
    //    {
    //        row.IsEditing = false;
    //        row.IsCreating = false;

    //        await UpdateResponseAsync();
    //    }
    //}

    //async Task UpdateProjectStartDate(ProjectResponse projectResponse)
    //{
    //    Project = projectResponse;
    //    Response.Deliverables.ForEach(d => { d.SetProjectTStartDate(projectResponse.InitialProjectDate); });
    //    await UpdateResponseAsync();
    //}
}
