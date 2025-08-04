using Blazored.FluentValidation;
using MudBlazor;
using MudBlazorWeb.FinishingLines.Products;
using Shared.Models.FinishingLines.Products;
using Shared.Models.FinishingLines.SKUs;
using static MudBlazor.CategoryTypes;

namespace MudBlazorWeb.FinishingLines.SKUs;
public partial class SKUDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; } = false;
    async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }


    FluentValidationValidator _fluentValidationValidator = null!;

    private async Task Submit()
    {
        var result = await GenericService.Post(Model);


        if (result.Succeeded)
        {
            _snackBar.ShowSuccess(result.Messages);
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            _snackBar.ShowError(result.Messages);
        }

    }


    private void Cancel() => MudDialog.Cancel();

    [Parameter]
    public SKUResponse Model { get; set; } = new();

    ProductResponseList _productResponseList = new();
    protected override async Task OnInitializedAsync()
    {
        await GetAllProducts();
    }
    async Task GetAllProducts()
    {
        var result = await GenericService.GetAll<ProductResponseList, ProductGetAll>(new ProductGetAll());
        if (result.Succeeded)
        {
            _productResponseList = result.Data;
        }
    }
    private Task<IEnumerable<ProductResponse>> SearchProduct(string value, CancellationToken token)
    {
        Func<ProductResponse, bool> Criteria = x =>
        x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase)
        ;
        IEnumerable<ProductResponse> FilteredItems = string.IsNullOrEmpty(value) ? _productResponseList.Items.AsEnumerable() :
             _productResponseList.Items.Where(Criteria);
        return Task.FromResult(FilteredItems);
    }
    public async Task AddProduct()
    {

        var parameters = new DialogParameters<ProductDialog>
        {

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<ProductDialog>("Product", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAllProducts();

        }
    }
}
