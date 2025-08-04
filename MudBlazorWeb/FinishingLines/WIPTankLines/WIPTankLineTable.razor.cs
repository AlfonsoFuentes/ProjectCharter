using MudBlazor;
using MudBlazorWeb.Templates;
using Shared.Models.FinishingLines.WIPTankLines;

namespace MudBlazorWeb.FinishingLines.WIPTankLines;
public partial class WIPTankLineTable
{
    [Parameter]
    public List<WIPTankLineResponse> Items { get; set; } = new();
    [Parameter]
    public EventCallback<List<WIPTankLineResponse>> ItemsChanged { get; set; }
    [Parameter]
    public EventCallback GetAll { get; set; }
    [Parameter]
    public EventCallback ValidateAsync { get; set; }
    [Parameter]
    public Guid LineId { get; set; }

    string nameFilter = string.Empty;
    public Func<WIPTankLineResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<WIPTankLineResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    //protected override async Task OnInitializedAsync()
    //{
    //    await GetAll();
    //}
    //async Task GetAll()
    //{
    //    var result = await GenericService.GetAll<WIPTankLineResponseList, WIPTankLineGetAll>(new WIPTankLineGetAll());
    //    if (result.Succeeded)
    //    {
    //        Items = result.Data.Items;
    //    }
    //}
    public async Task AddNew()
    {
        WIPTankLineResponse response = new()
        {
            LineId = LineId,
        };

        var parameters = new DialogParameters<WIPTankLineDialog>
        {
             { x => x.Model, response },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<WIPTankLineDialog>("WIPTankLine", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            if (LineId != Guid.Empty)
            {
                await GetAll.InvokeAsync();
            }
            else
            {
                Items.Add(response);
                await ItemsChanged.InvokeAsync(Items);
                await ValidateAsync.InvokeAsync();
            }
        }
    }
    async Task Edit(WIPTankLineResponse response)
    {


        var parameters = new DialogParameters<WIPTankLineDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };


        var dialog = await DialogService.ShowAsync<WIPTankLineDialog>("WIPTankLine", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            if (LineId != Guid.Empty)
            {
                await GetAll.InvokeAsync();
            }
        }
    }
    public async Task Delete(WIPTankLineResponse response)
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
            if (LineId != Guid.Empty)
            {
                DeleteWIPTankLineRequest request = new()
                {
                   LineId = LineId,
                   Id = response.Id,
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
                Items.Remove(response);
                await ItemsChanged.InvokeAsync(Items);
                await ValidateAsync.InvokeAsync();
            }
        }

    }
    HashSet<WIPTankLineResponse> SelecteItems = null!;
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
            DeleteGroupWIPTankLineRequest request = new()
            {
                SelecteItems = SelecteItems,
                LineId = LineId,
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

    }
}
