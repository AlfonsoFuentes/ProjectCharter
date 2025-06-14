namespace Web.Pages.ProjectDependant.Deliverables;
public partial class DeliverablesTable
{
    //[Parameter]
    //public DeliverableWithGanttTaskResponseList Response { get; set; } = new();

    //async Task GetAll()
    //{

    //    var result = await GenericService.GetAll<DeliverableWithGanttTaskResponseListToUpdate, GanttTaskGetAll>(new GanttTaskGetAll
    //    {
    //        ProjectId = Project.Id,
    //    });

    //    if (result.Succeeded)
    //    {

    //        Response = result.Data.ToReponse();
    //        Items = Response.Deliverables.Select(x => new DeliverableResponse()
    //        {
    //            Id = x.DeliverableId,
    //            Name = x.Name,
    //            ProjectId = x.ProjectId,

    //        }).ToList();
    //        StateHasChanged();

    //    }

    //}

    //[Parameter]
    //public ProjectResponse Project { get; set; } = null!;
    //public List<DeliverableResponse> Items { get; set; } = new();
    //string nameFilter = string.Empty;
    //public Func<DeliverableResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    //public List<DeliverableResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
    //    Items.Where(Criteria).ToList();
    //protected override async Task OnParametersSetAsync()
    //{
    //    await GetAll();
    //}

    //public async Task AddNew()
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



    //async Task Edit(DeliverableResponse response)
    //{


    //    var parameters = new DialogParameters<DeliverableDialog>
    //    {

    //         { x => x.Model, response },
    //    };
    //    var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


    //    var dialog = await DialogService.ShowAsync<DeliverableDialog>("Deliverable", parameters, options);
    //    var result = await dialog.Result;
    //    if (result != null)
    //    {
    //        await GetAll();
    //    }
    //}
    //async Task Delete(DeliverableResponse response)
    //{
    //    var parameters = new DialogParameters<DialogTemplate>
    //    {
    //        { x => x.ContentText, $"Do you really want to delete {response.Name}? This process cannot be undone." },
    //        { x => x.ButtonText, "Delete" },
    //        { x => x.Color, Color.Error }
    //    };

    //    var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

    //    var dialog = await DialogService.ShowAsync<DialogTemplate>("Delete", parameters, options);
    //    var result = await dialog.Result;


    //    if (!result!.Canceled)
    //    {
    //        DeleteDeliverableRequest request = new()
    //        {
    //            Id = response.Id,
    //            Name = response.Name,

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
    //HashSet<DeliverableResponse> SelecteItems = null!;
    //public async Task DeleteGroup()
    //{
    //    if (SelecteItems == null) return;
    //    var parameters = new DialogParameters<DialogTemplate>
    //    {
    //        { x => x.ContentText, $"Do you really want to delete this {SelecteItems.Count} Items? This process cannot be undone." },
    //        { x => x.ButtonText, "Delete" },
    //        { x => x.Color, Color.Error }
    //    };

    //    var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

    //    var dialog = await DialogService.ShowAsync<DialogTemplate>("Delete", parameters, options);
    //    var result = await dialog.Result;


    //    if (!result!.Canceled)
    //    {
    //        DeleteGroupDeliverableRequest request = new()
    //        {
    //            SelecteItems = SelecteItems,
    //            ProjectId = Project.Id,

    //        };
    //        var resultDelete = await GenericService.Post(request);
    //        if (resultDelete.Succeeded)
    //        {
    //            await GetAll();
    //            _snackBar.ShowSuccess(resultDelete.Messages);
    //            SelecteItems = null!;

    //        }
    //        else
    //        {
    //            _snackBar.ShowError(resultDelete.Messages);
    //        }
    //    }

    //}
    //DeliverableResponse SelectedRow = null!;
    //bool DisableUpButton => SelectedRow == null ? true : SelectedRow.Order == 1;
    //bool DisableDownButton => SelectedRow == null ? true : SelectedRow.Order == LastOrder;
    //public int LastOrder => Items.Count == 0 ? 1 : Items.MaxBy(x => x.Order)!.Order;
    //void RowClicked(DeliverableResponse item)
    //{
    //    SelectedRow = SelectedRow == null ? SelectedRow = item : SelectedRow = null!;
    //}
    //async Task Up()
    //{
    //    if (SelectedRow == null) return;


    //    var result = await GenericService.Update(SelectedRow.ToUp());
    //    if (result.Succeeded)
    //    {
    //        await GetAll();
    //    }
    //}
    //async Task Down()
    //{
    //    if (SelectedRow == null) return;

    //    var result = await GenericService.Update(SelectedRow.ToDown());

    //    if (result.Succeeded)
    //    {
    //        await GetAll();
    //    }
    //}

}
