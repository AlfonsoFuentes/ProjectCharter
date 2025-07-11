
using MudBlazor;
using MudBlazorWeb.Pages.PurchaseOrders.Dialogs;
using MudBlazorWeb.Templates;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Responses;

namespace MudBlazorWeb.Pages.PurchaseOrders.Tables;
public partial class PurchaseOrdersTable
{
    [Parameter]
    public string TableTitle { get; set; } = string.Empty;

    string nameFilter = string.Empty;
    public Func<PurchaseOrderResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
   x.SupplierName.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
   x.SupplierNickName.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
   x.PONumber.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
   x.PurchaseRequisition.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
    x.ProjectAccount.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
   x.CostCenter.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
   x.PurchaseOrderStatus.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<PurchaseOrderResponse> FilteredItemsByDate => ExpectedByDate ? Items.Where(CriteriaByExpectedCurrentDate).ToList() : Items;
    public List<PurchaseOrderResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? FilteredItemsByDate :
        FilteredItemsByDate.Where(Criteria).ToList();
    Func<PurchaseOrderResponse, bool> CriteriaByExpectedCurrentDate => x => x.ExpectedDate!.Value.Date < ExpectedDate!.Value.Date;
    DateTime? ExpectedDate = DateTime.UtcNow;
    [Parameter]
    public List<PurchaseOrderResponse> Items { get; set; } = new();
    [Parameter]
    public bool ShowToolbar { get; set; } = true;
    [Parameter]
    public bool HideStatus { get; set; } = false;
    [Parameter]
    public bool HidePR { get; set; } = false;
    [Parameter]
    public bool HideExpectedDate { get; set; } = true;
    [Parameter]
    public bool HidePONumber { get; set; } = false;
    [Parameter]
    public bool HideAccount { get; set; } = false;
    [Parameter]
    public bool HideSupplier { get; set; } = false;
    [Parameter]
    public bool ShowPrint { get; set; } = true;
    void ChangeFilterByDate(bool value)
    {
        ExpectedByDate = value;

        StateHasChanged();
    }

    [Parameter, EditorRequired]
    public EventCallback GetAll { get; set; } = default!;
    bool ExpectedByDate { get; set; } = false;

    async Task EditPurchaseOrderCreated(PurchaseOrderResponse purchaseOrder)
    {
        var parameters = new DialogParameters<EditPurchaseOrderCreatedDialog>
        {
            { x => x.PurchaseOrder, purchaseOrder},

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraLarge };

        var dialog = await DialogService.ShowAsync<EditPurchaseOrderCreatedDialog>("Edit Purchase Order Created", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll.InvokeAsync();
            StateHasChanged();
        }
    }
    [Parameter]
    public RenderFragment ExtraFilters { get; set; } = default!;
    async Task ApprovePurchaseOrder(PurchaseOrderResponse purchaseOrder)
    {
        var parameters = new DialogParameters<ApprovePurchaseDialog>
        {
            { x => x.PurchaseOrder, purchaseOrder},

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraLarge };

        var dialog = await DialogService.ShowAsync<ApprovePurchaseDialog>("Approve Purchase Order", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll.InvokeAsync();
            StateHasChanged();
        }
    }

    async Task EditPurchaseOrderApproved(PurchaseOrderResponse purchaseOrder)
    {
        if (purchaseOrder.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Approved.Id)
        {
            var parameters = new DialogParameters<EditPurchaseOrderApprovedDialog>
        {
            { x => x.PurchaseOrder, purchaseOrder},

        };

            var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraLarge };

            var dialog = await DialogService.ShowAsync<EditPurchaseOrderApprovedDialog>("Approve Purchase Order", parameters, options);
            var result = await dialog.Result;
            if (result != null)
            {
                await GetAll.InvokeAsync();
                StateHasChanged();
            }
        }
        else if (purchaseOrder.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Receiving.Id)
        {
            var parameters = new DialogParameters<EditPurchaseOrderClosedDialog>
        {
            { x => x.PurchaseOrder, purchaseOrder},

        };

            var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraLarge };

            var dialog = await DialogService.ShowAsync<EditPurchaseOrderClosedDialog>("Edit Purchase Order Receiving", parameters, options);
            var result = await dialog.Result;
            if (result != null)
            {
                await GetAll.InvokeAsync();
                StateHasChanged();
            }
        }

    }
    async Task ReceivePurchaseOrder(PurchaseOrderResponse purchaseOrder)
    {

        var parameters = new DialogParameters<ReceivePurchaseOrderDialog>
        {
            { x => x.PurchaseOrder, purchaseOrder},

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraLarge };

        var dialog = await DialogService.ShowAsync<ReceivePurchaseOrderDialog>("Receive Purchase Order", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll.InvokeAsync();
            StateHasChanged();
        }
    }

    async Task EditPurchaseOrderClosed(PurchaseOrderResponse purchaseOrder)
    {



        var parameters = new DialogParameters<EditPurchaseOrderClosedDialog>
        {
            { x => x.PurchaseOrder, purchaseOrder},

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraLarge };

        var dialog = await DialogService.ShowAsync<EditPurchaseOrderClosedDialog>("Edit Purchase Order Closed", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll.InvokeAsync();
            StateHasChanged();
        }
    }
    async Task EditPurchaseOrderSalary(PurchaseOrderResponse purchaseOrder)
    {



        var parameters = new DialogParameters<EditPurchaseOrderSalaryDialog>
        {
            { x => x.PurchaseOrder, purchaseOrder},

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<EditPurchaseOrderSalaryDialog>("Edit Purchase Order Salary", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll.InvokeAsync();
            StateHasChanged();
        }
    }

    public async Task Delete(PurchaseOrderResponse response)
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
            DeletePurchaseOrderRequest request = new()
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

    }
}
