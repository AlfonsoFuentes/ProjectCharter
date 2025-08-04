using MudBlazor;
using MudBlazorWeb.Templates;
using Shared.Models.FinishingLines.MixerBackbones;

namespace MudBlazorWeb.FinishingLines.MixerBackbones;
public partial class MixerBackboneTable
{
    [Parameter]
    public List<MixerBackboneResponse> Items { get; set; } = new();
    [Parameter]
    public EventCallback<List<MixerBackboneResponse>> ItemsChanged { get; set; }
    string nameFilter = string.Empty;
    public Func<MixerBackboneResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<MixerBackboneResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    [Parameter]
    public EventCallback GetAll { get; set; }
    [Parameter]
    public Guid MixerId { get; set; } = Guid.Empty;
    [Parameter]
    public EventCallback ValidateAsync { get; set; }
    public async Task AddNew()
    {
        MixerBackboneResponse response = new()
        {

            MixerId = MixerId,


        };
        var parameters = new DialogParameters<MixerBackboneDialog>
        {
             { x => x.Model, response },

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<MixerBackboneDialog>("MixerBackbone", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            if (MixerId != Guid.Empty)
            {
                await GetAll.InvokeAsync();
            }
            else
            {
                Items.Add(response);
                await ItemsChanged.InvokeAsync(Items);
                await ValidateAsync.InvokeAsync();
            }


            StateHasChanged();
        }
    }
    async Task Edit(MixerBackboneResponse response)
    {


        var parameters = new DialogParameters<MixerBackboneDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };


        var dialog = await DialogService.ShowAsync<MixerBackboneDialog>("MixerBackbone", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            if (MixerId != Guid.Empty)
            {
                await GetAll.InvokeAsync();
            }
        }
    }
    public async Task Delete(MixerBackboneResponse response)
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
            if (MixerId != Guid.Empty)
            {
                DeleteMixerBackboneRequest request = new()
                {
                    MixerId = MixerId,
                    BackboneId = response.Backbone!.Id,
                    Name = response.Name,

                };
                var resultDelete = await GenericService.Post(request);
                if (resultDelete.Succeeded)
                {
                    await GetAll.InvokeAsync();
                    _snackBar.ShowSuccess(resultDelete.Messages);


                }
                else
                {
                    _snackBar.ShowError(resultDelete.Messages);
                }
            }
            else
            {
                await ItemsChanged.InvokeAsync(Items);
                await ValidateAsync.InvokeAsync();
                _snackBar.ShowSuccess($"Item {response.Name} deleted successfully.");
            }

        }

    }
    HashSet<MixerBackboneResponse> SelecteItems = null!;
    public async Task DeleteGroup()
    {
        if (SelecteItems == null) return;
        var parameters = new DialogParameters<DialogTemplate>
        {
            { x => x.ContentText, $"Do you really want to delete this {SelecteItems.Count} Items? This process cannot be undone." },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DialogTemplate>("Delete", parameters, options);
        var result = await dialog.Result;


        if (!result!.Canceled)
        {
            if (MixerId != Guid.Empty)
            {
                DeleteGroupMixerBackboneRequest request = new()
                {
                    SelecteItems = SelecteItems,
                    MixerId = MixerId,

                };
                var resultDelete = await GenericService.Post(request);
                if (resultDelete.Succeeded)
                {
                    await GetAll.InvokeAsync();
                    _snackBar.ShowSuccess(resultDelete.Messages);
                    SelecteItems = null!;

                }
                else
                {
                    _snackBar.ShowError(resultDelete.Messages);
                }
            }
            else
            {
                Items.RemoveAll(x => SelecteItems.Contains(x));
                await ItemsChanged.InvokeAsync(Items);
                await ValidateAsync.InvokeAsync();
                _snackBar.ShowSuccess($"Group of {SelecteItems.Count} items deleted successfully.");
                SelecteItems = null!;
            }
        }

    }
}
