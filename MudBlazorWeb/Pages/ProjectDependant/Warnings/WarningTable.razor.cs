using MudBlazor;
using MudBlazorWeb.Pages.ProjectDependant.MonitoringGanttTasks;
using MudBlazorWeb.Pages.PurchaseOrders.Dialogs;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.MonitoringTask.Responses;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.Warnings.Records;
using Shared.Models.Warnings.Responses;

namespace MudBlazorWeb.Pages.ProjectDependant.Warnings;
public partial class WarningTable
{
    [Parameter,EditorRequired]
    public List<WarningResponse> Items { get; set; } = new();

    string nameFilter = string.Empty;
    public Func<WarningResponse, bool> Criteria => x =>
    x.WarningText.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
     x.ProjectNumber.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
     x.ProjectName.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) 
    ;
    public List<WarningResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    [Parameter,EditorRequired]
    public EventCallback GetAll { get; set; }
   
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
    async Task EditGanttTask(MonitoringGanttTaskResponse editrow)
    {
        //EditRow = editrow;
        if (editrow != null)
        {
            var parameters = new DialogParameters<MonitoringGanttTaskDialog>
            {

                { x => x.Response, editrow },


            };
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
            var dialog = await DialogService.ShowAsync<MonitoringGanttTaskDialog>("Edit current Dates", parameters, options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {

                await GetAll.InvokeAsync();
            }

        }

    }
}
