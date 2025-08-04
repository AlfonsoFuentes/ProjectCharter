using MudBlazor;
using MudBlazorWeb.Templates;
using Shared.Models.FinishingLines.ProductionScheduleItems;

namespace MudBlazorWeb.FinishingLines.ProductionScheduleItems;
public partial class ProductionScheduleItemTable
{
    [Parameter]
    public List<ProductionScheduleItemResponse> Items { get; set; } = new();
    public List<ProductionScheduleItemResponse> OrderedItems => Items.OrderBy(x => x.Order).ToList();
    int LastOrder => OrderedItems.Count > 0 ? OrderedItems.Max(x => x.Order) : 0;
    [Parameter]
    public EventCallback<List<ProductionScheduleItemResponse>> ItemsChanged { get; set; }
    [Parameter]
    public EventCallback GetAll { get; set; }
    [Parameter]
    public EventCallback ValidateAsync { get; set; }
    [Parameter]
    public Guid ProductionLineAssigmentId { get; set; } = Guid.Empty;
    [Parameter]
    public Guid ProductiontLineId { get; set; } = Guid.Empty;

    string nameFilter = string.Empty;
    public Func<ProductionScheduleItemResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<ProductionScheduleItemResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? OrderedItems :
        OrderedItems.Where(Criteria).ToList();
    //protected override async Task OnInitializedAsync()
    //{
    //    await GetAll();
    //}
    //async Task GetAll()
    //{
    //    var result = await GenericService.GetAll<ProductionScheduleItemResponseList, ProductionScheduleItemGetAll>(new ProductionScheduleItemGetAll());
    //    if (result.Succeeded)
    //    {
    //        Items = result.Data.Items;
    //    }
    //}
    public async Task AddNew()
    {
        ProductionScheduleItemResponse response = new()
        {
            ProductionLineAssigmentId = ProductionLineAssigmentId,
            ProductionLineId = ProductiontLineId,
            Order = LastOrder + 1,
        };

        var parameters = new DialogParameters<ProductionScheduleItemDialog>
        {
              { x => x.Model, response },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<ProductionScheduleItemDialog>("ProductionScheduleItem", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            if (ProductionLineAssigmentId != Guid.Empty)
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
    async Task Edit(ProductionScheduleItemResponse response)
    {
        response.ProductionLineId = ProductiontLineId;

        var parameters = new DialogParameters<ProductionScheduleItemDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };


        var dialog = await DialogService.ShowAsync<ProductionScheduleItemDialog>("ProductionScheduleItem", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            if (ProductionLineAssigmentId != Guid.Empty)
            {
                await GetAll.InvokeAsync();
            }
            else
            {

                await ItemsChanged.InvokeAsync(Items);
                await ValidateAsync.InvokeAsync();
            }

        }
    }
    public async Task Delete(ProductionScheduleItemResponse response)
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
            if (ProductionLineAssigmentId != Guid.Empty)
            {
                DeleteProductionScheduleItemRequest request = new()
                {
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
                _snackBar.ShowSuccess($"Item {response.Name} deleted successfully.");
            }
        }

    }


    HashSet<ProductionScheduleItemResponse> SelecteItems = null!;
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
            if (ProductionLineAssigmentId != Guid.Empty)
            {
                DeleteGroupProductionScheduleItemRequest request = new()
                {
                    SelecteItems = SelecteItems,

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
                foreach (var item in SelecteItems)
                {
                    Items.Remove(item);
                }
                await ItemsChanged.InvokeAsync(Items);
                await ValidateAsync.InvokeAsync();
                _snackBar.ShowSuccess($"Group of {SelecteItems.Count} Items deleted successfully.");
                SelecteItems = null!;
            }

        }

    }
}
