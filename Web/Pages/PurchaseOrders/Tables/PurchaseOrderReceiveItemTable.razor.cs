using Shared.Enums.CurrencyEnums;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Responses;

namespace Web.Pages.PurchaseOrders.Tables;
public partial class PurchaseOrderReceiveItemTable
{

 
    
    [Parameter]
    public List<PurchaseOrderItemResponse> Items { get; set; } = new();
    public List<PurchaseOrderItemResponse> OrderedItems => Items.Count==0 ? new() : Items.OrderBy(x => x.Order).ToList();
    [Parameter]
    public EventCallback<List<PurchaseOrderItemResponse>> ItemsChanged { get; set; }


    [Parameter, EditorRequired]
    public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
    async Task OnItemsChanged()
    {
        if (ItemsChanged.HasDelegate)
        {
            await ItemsChanged.InvokeAsync(Items);
        }
    }
    [Parameter]
    public EventCallback ValidateAsync { get; set; }
    [Parameter]
    public EventCallback SelectItemChanged { get; set; }

    
    

}
