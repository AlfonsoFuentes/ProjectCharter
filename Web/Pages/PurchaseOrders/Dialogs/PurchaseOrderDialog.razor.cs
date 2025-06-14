using Shared.Models.BudgetItems.Responses;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.Suppliers.Responses;

namespace Web.Pages.PurchaseOrders.Dialogs;
public partial class PurchaseOrderDialog
{
    [Inject]
    public IRate _CurrencyService { get; set; } = null!;
    [Parameter]
    public PurchaseOrderResponse Model { get; set; } = null!;
    [Parameter]
    public bool ShowReceive { get; set; } = false;
    [Parameter]
    public bool ShowEditReceive { get; set; } = false;
    public ConversionRate RateList { get; set; } = null!;

  
    [Parameter]
    public EventCallback ValidateAsync { get; set; }
   
    void ChangeNamePO()
    {
        if (Model.SelectedPurchaseOrderItems.Count == 1)
            Model.PurchaseOrderItems[0].Name = Model.Name;

    }
    void ChangeBudgetItemName(string name)
    {
        if (Model.SelectedPurchaseOrderItems.Count == 1)
            Model.Name = name;
    }

    void ChangeSupplier()
    {
        if (Model.Supplier != null)
        {
            Model.PurchaseOrderCurrency = Model.Supplier.SupplierCurrency;
            Model.QuoteCurrency = Model.Supplier.SupplierCurrency;

        }
            
    }
    async Task DeleteItem(PurchaseOrderItemResponse selected)
    {
        Model.RemoveItem(selected);

        await ValidateAsync.InvokeAsync();
        
    }
    void AddBudgetItem()
    {

        Model.AddItem(new());
        StateHasChanged();
    }
    [Parameter]
    public List<BudgetItemWithPurchaseOrdersResponse> NonSelectedBudgetItems { get; set; } = new();
    public async Task ChangedCurrencyDate(DateTime? currentdate)
    {
        if (!currentdate.HasValue) return;
        if(currentdate.Value.Date > DateTime.UtcNow.Date)
        {
            _snackBar.ShowError($"Currency date must be lower than today {DateTime.UtcNow.Date}");
            Model.CurrencyDate = DateTime.UtcNow.Date;
            return; 
        }

        var result = await _CurrencyService.GetRates(currentdate.Value);
        if (result != null)
        {
            Model.USDCOP = result.COP;
            Model.USDEUR = result.EUR;
            StateHasChanged();
        }


    }
    [Parameter]
    public List<SupplierResponse> Suppliers { get; set; } = new();
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    [Parameter]
    public EventCallback AddSupplier { get; set; }

}
