using MudBlazor;
using MudBlazorWeb.Templates;
using Shared.Models.FinishingLines.LineSpeeds;
using Shared.Models.FinishingLines.WIPTankLines;
using static Shared.StaticClasses.StaticClass;

namespace MudBlazorWeb.FinishingLines.LineSpeeds;
public partial class LineSpeedTable
{
    [Parameter]
    public List<LineSpeedResponse> Items { get; set; } = new();
    [Parameter]
    public EventCallback<List<LineSpeedResponse>> ItemsChanged { get; set; }
    [Parameter]
    public EventCallback GetAll { get; set; }
    [Parameter]
    public EventCallback ValidateAsync { get; set; }
    [Parameter]
    public Guid LineId { get; set; }

    string nameFilter = string.Empty;
    public Func<LineSpeedResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<LineSpeedResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    //protected override async Task OnInitializedAsync()
    //{
    //    await GetAll();
    //}
    //async Task GetAll()
    //{
    //    var result = await GenericService.GetAll<LineSpeedResponseList, LineSpeedGetAll>(new LineSpeedGetAll());
    //    if (result.Succeeded)
    //    {
    //        Items = result.Data.Items;
    //    }
    //}
    public async Task AddNew()
    {
        LineSpeedResponse response = new()
        {
            LineId = LineId,
        };
        var parameters = new DialogParameters<LineSpeedDialog>
        {
            { x => x.Model, response },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<LineSpeedDialog>("LineSpeed", parameters, options);
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
    async Task Edit(LineSpeedResponse response)
    {


        var parameters = new DialogParameters<LineSpeedDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };


        var dialog = await DialogService.ShowAsync<LineSpeedDialog>("LineSpeed", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            if (LineId != Guid.Empty)
            {
                await GetAll.InvokeAsync();
            }
        }
    }
    public async Task Delete(LineSpeedResponse response)
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
                DeleteLineSpeedRequest request = new()
                {
                    LineId = response.LineId,
                    SkuId = response.Sku!.Id,
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
    HashSet<LineSpeedResponse> SelecteItems = null!;
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
            DeleteGroupLineSpeedRequest request = new()
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
