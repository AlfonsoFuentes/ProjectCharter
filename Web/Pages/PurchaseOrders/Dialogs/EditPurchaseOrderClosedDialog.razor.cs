using Blazored.FluentValidation;
using MudBlazor;
using Newtonsoft.Json.Linq;
using Shared.Models.PurchaseOrders.Mappers;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.Suppliers.Records;
using Shared.Models.Suppliers.Responses;
using Web.Pages.Suppliers;

namespace Web.Pages.PurchaseOrders.Dialogs;
public partial class EditPurchaseOrderClosedDialog
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

    public List<SupplierResponse> Suppliers { get; set; } = new();
    [Parameter]
    public PurchaseOrderResponse PurchaseOrder { get; set; } = new();
    public EditPurchaseOrderClosedRequest Model { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {

        await GetSuppliers();
        Model = PurchaseOrder.ToEditClosed();
        StateHasChanged();

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
    async Task GetSuppliers()
    {
        var result = await GenericService.GetAll<SupplierResponseList, SupplierGetAll>(new SupplierGetAll());
        if (result.Succeeded)
        {
            Suppliers = result.Data.Items;
        }
    }
    public async Task ChangedReceiveDate(DateTime? currentdate)
    {
        if (!currentdate.HasValue) return;
        if (currentdate.Value.Date > DateTime.UtcNow.Date)
        {
            _snackBar.ShowError($"Currency date must be lower than today {DateTime.UtcNow.Date}");
            Model.ReceivingDate = DateTime.UtcNow.Date;
            return;
        }

        var result = await _CurrencyService.GetRates(currentdate.Value);
        if (result != null)
        {
            Model.ReceivingUSDCOP = result.COP;
            Model.ReceivingUSDEUR = result.EUR;
            StateHasChanged();
        }


    }
    async Task ToReceiveValueChanged(string value)
    {
        _value = value;
        if (_value.Equals("Complete"))
        {
            foreach (var row in Model.PurchaseOrderItems)
            {
                row.ReceivingValueCurrency = row.CommitmentItemPurchaseOrderCurrency;



            }

        }
        else if (_value.Equals("Percentage"))
        {
            foreach (var row in Model.PurchaseOrderItems)
            {
                row.ReceivingValueCurrency = 0;



            }
        }
        await ValidateAsync();
    }
    double PercentageReceiving = 0;
    async Task PercentageChanged(double value)
    {
        PercentageReceiving = value;
        foreach (var row in Model.PurchaseOrderItems)
        {
            row.ReceivingValueCurrency = row.CommitmentItemPurchaseOrderCurrency * PercentageReceiving / 100.0;



        }
        await ValidateAsync();
    }

}
