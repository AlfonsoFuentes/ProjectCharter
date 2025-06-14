using Blazored.FluentValidation;
using MudBlazor;
using Shared.Enums.CurrencyEnums;
using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.Suppliers.Records;
using Shared.Models.Suppliers.Responses;
using Web.Pages.Suppliers;

namespace Web.Pages.PurchaseOrders.Dialogs;
public partial class CreatePurchaseOrderDialog
{
    [Inject]
    public IRate _CurrencyService { get; set; } = null!;
    public ConversionRate RateList { get; set; } = null!;
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
    public List<BudgetItemWithPurchaseOrdersResponse> OriginalBudgetItems => BudgetItem.IsAlteration ? ResponseList.Expenses : ResponseList.Capital;
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
    List<BudgetItemWithPurchaseOrdersResponse> NonSelectedBudgetItemsOrdered=> NonSelectedBudgetItems.OrderBy(x=>x.Nomenclatore).ToList();
    protected override async Task OnInitializedAsync()
    {
        RateList = await _CurrencyService.GetRates(DateTime.UtcNow);
        var USDCOP = RateList == null ? 4000 : Math.Round(RateList.COP, 2);
        var USDEUR = RateList == null ? 1 : Math.Round(RateList.EUR, 2);
        await GetSuppliers();

        NonSelectedBudgetItems = OriginalBudgetItems;
        Model.IsProductiveAsset = ResponseList.IsProductiveAsset;
        Model.CostCenter = ResponseList.CostCenter;
        Model.ProjectId = ResponseList.ProjectId;
        Model.ProjectAccount = ResponseList.ProjectNumber;
        Model.CurrencyDate = DateTime.UtcNow;

        PurchaseOrderItemResponse item = new();
        if (OriginalBudgetItems.Any(x => x.Id == BudgetItem.Id))
        {
            item.BudgetItem = OriginalBudgetItems.Single(x => x.Id == BudgetItem.Id);
            Model.AddItem(item);
            NonSelectedBudgetItems.Remove(item.BudgetItem!);
            Model.MainBudgetItemId = BudgetItem.Id;

        }


        Model.AddItem(new());
        Model.USDCOP = USDCOP;
        Model.USDEUR = USDEUR;
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
