using MudBlazor;
using MudBlazorWeb.Templates;
using Shared.Models.FinishingLines.Products;

namespace MudBlazorWeb.FinishingLines.Products;
public partial class ProductTable
{
    [Parameter]
    public List<ProductResponse> Items { get; set; } = new();
    [Parameter]
    public EventCallback<List<ProductResponse>> ItemsChanged { get; set; }
 
    [Parameter]
    public EventCallback ValidateAsync { get; set; }
    

    string nameFilter = string.Empty;
    public Func<ProductResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<ProductResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    protected override async Task OnInitializedAsync()
    {
        await GetAll();
    }
    async Task GetAll()
    {
        var result = await GenericService.GetAll<ProductResponseList, ProductGetAll>(new ProductGetAll());
        if (result.Succeeded)
        {
            Items = result.Data.Items;
        }
    }
    public async Task AddNew()
    {
        ProductResponse response = new()
        {
           
        };

        var parameters = new DialogParameters<ProductDialog>
        {
             { x => x.Model, response },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<ProductDialog>("Product", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            await GetAll();
        }
    }
    async Task Edit(ProductResponse response)
    {


        var parameters = new DialogParameters<ProductDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };


        var dialog = await DialogService.ShowAsync<ProductDialog>("Product", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            await GetAll();
        }
    }
    public async Task Delete(ProductResponse response)
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
            DeleteProductRequest request = new()
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
    HashSet<ProductResponse> SelecteItems = null!;
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
            DeleteGroupProductRequest request = new()
            {
                SelecteItems = SelecteItems,

            };
            var resultDelete = await GenericService.Post(request);
            if (resultDelete.Succeeded)
            {
                await GetAll();
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
