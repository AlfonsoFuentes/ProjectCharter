using Blazored.FluentValidation;
using MudBlazor;
using MudBlazorWeb.Pages.Suppliers;
using MudBlazorWeb.Services.CurrencyServices;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.Suppliers.Records;
using Shared.Models.Suppliers.Responses;

namespace MudBlazorWeb.Pages.PurchaseOrders.Dialogs;
public partial class CreatePurchaseOrderDialog
{
    
 
    FluentValidationValidator _fluentValidationValidator = null!;
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; } = false;
    async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }
    [Parameter]
    public BudgetItemWithPurchaseOrdersResponse BudgetItem { get; set; } = null!;
    CreatePurchaseOrderRequest Model { get; set; } = new();
    [Parameter]
    public BudgetItemWithPurchaseOrderResponseList ResponseList { get; set; } = new();
    public List<BudgetItemWithPurchaseOrdersResponse> OriginalBudgetItems => BudgetItem.IsAlteration ? ResponseList.Expenses : ResponseList.CapitalPlusContingency;
    List<BudgetItemWithPurchaseOrdersResponse> NonSelectedBudgetItems = new();
    public List<SupplierResponse> Suppliers { get; set; } = new();
    async Task GetSuppliers()
    {
        var result = await GenericService.GetAll<SupplierResponseList, SupplierGetAll>(new SupplierGetAll());
        if (result.Succeeded)
        {
            Suppliers = result.Data.Items;
        }
    }
    List<BudgetItemWithPurchaseOrdersResponse> NonSelectedBudgetItemsOrdered => NonSelectedBudgetItems.OrderBy(x => x.Nomenclatore).ToList();
    [Inject]
    public INewCurrency _CurrencyService { get; set; } = null!;
    public  ConversionRate ConversionRate { get; set; } = null!;
    protected override async Task OnInitializedAsync()
    {
        ConversionRate=await _CurrencyService.GetRates(DateTime.UtcNow);
        var USDCOP = Math.Round(ConversionRate.COP, 2);
        var USDEUR = Math.Round(ConversionRate.EUR, 2);
        await GetSuppliers();

        NonSelectedBudgetItems = OriginalBudgetItems;
        Model.IsProductiveAsset = ResponseList.IsProductiveAsset;
        Model.CostCenter = ResponseList.CostCenter;
        Model.ProjectId = ResponseList.ProjectId;
        Model.ProjectAccount = ResponseList.ProjectNumber;
        Model.CurrencyDate = DateTime.UtcNow;
        Model.IsAlteration = BudgetItem.IsAlteration;
        Model.IsTaxEditable = BudgetItem.IsTaxes;

        PurchaseOrderItemResponse item = new();
        if (OriginalBudgetItems.Any(x => x.Id == BudgetItem.Id))
        {
            item.BudgetItem = OriginalBudgetItems.Single(x => x.Id == BudgetItem.Id);
            Model.AddItem(item);
            NonSelectedBudgetItems.Remove(item.BudgetItem!);
            Model.MainBudgetItemId = BudgetItem.Id;
            item.Quantity = 1;
        }


        Model.AddItem(new());
        Model.USDCOP = USDCOP;
        Model.USDEUR = USDEUR;
        Model.ProjectPurchaseOrders = ResponseList.ProjectPurchaseOrders;
    }



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
    public async Task AddSupplier()
    {

        var parameters = new DialogParameters<SupplierDialog>
        {

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<SupplierDialog>("Supplier", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetSuppliers();

        }
    }

}
