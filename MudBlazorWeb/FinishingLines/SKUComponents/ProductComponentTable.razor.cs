using MudBlazor;
using MudBlazorWeb.Templates;
using Shared.Models.FinishingLines.ProductComponents;

namespace MudBlazorWeb.FinishingLines.SKUComponents;
public partial class ProductComponentTable
{
    [Parameter]
    public List<ProductComponentResponse> Items { get; set; } = new();
    [Parameter]
    public EventCallback<List<ProductComponentResponse>> ItemsChanged { get; set; }
    [Parameter]
    public EventCallback GetAll { get; set; }
    [Parameter]
    public EventCallback ValidateAsync { get; set; }
    [Parameter]
    public Guid ProductId { get; set; } = Guid.Empty;
    [Parameter]
    public RenderFragment Buttons { get; set; } = null!;
    string nameFilter = string.Empty;
    public Func<ProductComponentResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<ProductComponentResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    //protected override async Task OnInitializedAsync()
    //{
    //    await GetAll();
    //}
    //async Task GetAll()
    //{
    //    var result = await GenericService.GetAll<SKUComponentResponseList, SKUComponentGetAll>(new SKUComponentGetAll());
    //    if (result.Succeeded)
    //    {
    //        Items = result.Data.Items;
    //    }
    //}
    public async Task AddNew()
    {
        ProductComponentResponse response = new()
        {
            ProductId = ProductId
        };

        var parameters = new DialogParameters<ProductComponentDialog>
        {
             { x => x.Model, response },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<ProductComponentDialog>("Product component", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            if (ProductId != Guid.Empty)
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
    async Task Edit(ProductComponentResponse response)
    {


        var parameters = new DialogParameters<ProductComponentDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };


        var dialog = await DialogService.ShowAsync<ProductComponentDialog>("SKUComponent", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            if (ProductId != Guid.Empty)
            {
                await GetAll.InvokeAsync();
            }
        }
    }
    public async Task Delete(ProductComponentResponse response)
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
            if (ProductId != Guid.Empty)
            {
                DeleteProductComponentRequest request = new()
                {
                    BackBoneId = response.Backbone!.Id,
                    Name = response.Name,
                    ProductId = ProductId

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
            {
                Items.Remove(response);
                await ItemsChanged.InvokeAsync(Items);
                await ValidateAsync.InvokeAsync();
            }
        }

    }
    HashSet<ProductComponentResponse> SelecteItems = null!;
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
            if (ProductId != Guid.Empty)
            {
                DeleteGroupProductComponentRequest request = new()
                {
                    SelecteItems = SelecteItems,
                    ProductId = ProductId,
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
                _snackBar.ShowSuccess($"Group of {SelecteItems.Count} Items deleted successfully.");
                SelecteItems = null!;
            }
        }

    }
}
