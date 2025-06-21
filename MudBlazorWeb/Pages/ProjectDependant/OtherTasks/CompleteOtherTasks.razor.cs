using MudBlazor;
using MudBlazorWeb.Templates;
using Shared.Models.OtherTasks.Records;
using Shared.Models.OtherTasks.Requests;
using Shared.Models.OtherTasks.Responses;

namespace MudBlazorWeb.Pages.ProjectDependant.OtherTasks;
public partial class CompleteOtherTasks
{
    public List<OtherTaskResponse> Items { get; set; } = new();
    string nameFilter = string.Empty;
    public Func<OtherTaskResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<OtherTaskResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    protected override async Task OnParametersSetAsync()
    {
        await GetAll();
    }
    async Task GetAll()
    {
  
        var result = await GenericService.GetAll<OtherTaskResponseList, OtherTaskGetAll>(new OtherTaskGetAll());
        if (result.Succeeded)
        {
            Items = result.Data.Items;
        }
    }
    OtherTaskResponse SelectedRow = null!;
    bool DisableUpButton => SelectedRow == null ? true : SelectedRow.Order == 1;
    bool DisableDownButton => SelectedRow == null ? true : SelectedRow.Order == LastOrder;
    public int LastOrder => Items.Count == 0 ? 1 : Items.MaxBy(x => x.Order)!.Order;
    void RowClicked(OtherTaskResponse item)
    {
        SelectedRow = SelectedRow == null ? SelectedRow = item : SelectedRow = null!;
    }
    HashSet<OtherTaskResponse> SelecteItems = null!;

    async Task Edit(OtherTaskResponse response)
    {


        var parameters = new DialogParameters<OtherTaskDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<OtherTaskDialog>("OtherTask", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
        }
    }
    public async Task Delete(OtherTaskResponse response)
    {
        var parameters = new DialogParameters<DialogTemplate>
        {
            { x => x.ContentText, $"Do you really want to delete {response.Name}? This process cannot be undone." },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DialogTemplate>("Delete", parameters, options);
        var result = await dialog.Result;


        if (!result!.Canceled)
        {
            DeleteOtherTaskRequest request = new()
            {
                Id = response.Id,
                Name = response.Name,

            };
            var resultDelete = await GenericService.Post(request);
            if (resultDelete.Succeeded)
            {
                await GetAll();
                _snackBar.ShowSuccess(resultDelete.Messages);


            }
            else
            {
                _snackBar.ShowError(resultDelete.Messages);
            }
        }

    }
}
