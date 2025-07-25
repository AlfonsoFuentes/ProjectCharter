using Blazored.FluentValidation;
using MudBlazor;
using MudBlazorWeb.Pages.Suppliers;
using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.PurchaseOrders.Mappers;
using Shared.Models.PurchaseOrders.Records;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.Suppliers.Records;
using Shared.Models.Suppliers.Responses;

namespace MudBlazorWeb.Pages.PurchaseOrders.Dialogs;
public partial class ApprovePurchaseDialog
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
    public List<SupplierResponse> Suppliers { get; set; } = new();
    List<BudgetItemWithPurchaseOrdersResponse> NonSelectedBudgetItemsOrdered => NonSelectedBudgetItems.OrderBy(x => x.Nomenclatore).ToList();
    protected override async Task OnInitializedAsync()
    {
        await GetPurchaseOrder();
        await GetSuppliers();
        await GetBudgetItems();
        Model = PurchaseOrder.ToApprove();
        Model.ExpectedDate = DateTime.UtcNow;
        foreach (var item in Model.PurchaseOrderItems)
        {
            item.BudgetItem = OriginalBudgetItems.SingleOrDefault(y => y.Id == item.BudgetItemId);
        }


        NonSelectedBudgetItems.Remove(OriginalBudgetItems.Single(y => y.Id == Model.MainBudgetItemId));

        StateHasChanged();
        Model.ProjectPurchaseOrders = ResponseList.ProjectPurchaseOrders;
    }
    async Task GetPurchaseOrder()
    {
        var result = await GenericService.GetById<PurchaseOrderResponse,
            GetPurchaseOrderByIdRequest>(new GetPurchaseOrderByIdRequest()
            {
                Id = PurchaseOrder.Id,
            });
        if (result.Succeeded)
        {
            PurchaseOrder = result.Data;
        }
    }
    public BudgetItemWithPurchaseOrderResponseList ResponseList { get; set; } = new();
    async Task GetBudgetItems()
    {
        var resultProjectt = await GenericService.GetAll<BudgetItemWithPurchaseOrderResponseList, BudgetItemWithPurchaseOrderGetAll>(new BudgetItemWithPurchaseOrderGetAll()
        {
            ProjectId = PurchaseOrder.ProjectId,
        });
        if (resultProjectt.Succeeded)
        {
            ResponseList = resultProjectt.Data;
            if (PurchaseOrder.IsAlteration)
            {
                OriginalBudgetItems = resultProjectt.Data.Expenses;
            }
            else
            {
                OriginalBudgetItems = resultProjectt.Data.CapitalPlusContingency;
            }
            NonSelectedBudgetItems = OriginalBudgetItems;
        }
    }
    List<BudgetItemWithPurchaseOrdersResponse> OriginalBudgetItems = new();
    List<BudgetItemWithPurchaseOrdersResponse> NonSelectedBudgetItems = new();
    [Parameter]
    public PurchaseOrderResponse PurchaseOrder { get; set; } = new();
    public ApprovePurchaseOrderRequest Model { get; set; } = new();


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
