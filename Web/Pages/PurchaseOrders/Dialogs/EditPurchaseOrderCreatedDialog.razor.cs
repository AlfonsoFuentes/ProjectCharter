using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.PurchaseOrders.Mappers;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.Suppliers.Records;
using Shared.Models.Suppliers.Responses;
using Web.Pages.Suppliers;

namespace Web.Pages.PurchaseOrders.Dialogs;
public partial class EditPurchaseOrderCreatedDialog
{
    FluentValidationValidator _fluentValidationValidator = null!;
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; } = false;
    async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }

    List<BudgetItemWithPurchaseOrdersResponse> NonSelectedBudgetItemsOrdered => NonSelectedBudgetItems.OrderBy(x => x.Nomenclatore).ToList();
    public List<SupplierResponse> Suppliers { get; set; } = new();
    [Parameter]
    public PurchaseOrderResponse PurchaseOrder { get; set; } = new();
    public EditPurchaseOrderCreatedRequest Model { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {

        await GetSuppliers();
        await GetBudgetItems();
        Model = PurchaseOrder.ToEditCreated();
        Model.PurchaseOrderItems.ForEach(x =>
        {
            x.BudgetItem = OriginalBudgetItems.Single(y => y.Id == x.BudgetItemId);


        });
        NonSelectedBudgetItems.Remove(OriginalBudgetItems.Single(y => y.Id == Model.MainBudgetItemId));
        Model.AddItem(new());
        StateHasChanged();

    }
    async Task GetBudgetItems()
    {
        var resultProjectt = await GenericService.GetAll<BudgetItemWithPurchaseOrderResponseList, BudgetItemWithPurchaseOrderGetAll>(new BudgetItemWithPurchaseOrderGetAll()
        {
            ProjectId = PurchaseOrder.ProjectId,
        });
        if (resultProjectt.Succeeded)
        {
            if (Model.IsAlteration)
            {
                OriginalBudgetItems = resultProjectt.Data.Expenses;
            }
            else
            {
                OriginalBudgetItems = resultProjectt.Data.Capital;
            }
            NonSelectedBudgetItems = OriginalBudgetItems;
        }
    }
    List<BudgetItemWithPurchaseOrdersResponse> OriginalBudgetItems = new();
    List<BudgetItemWithPurchaseOrdersResponse> NonSelectedBudgetItems = new();
    


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
}
