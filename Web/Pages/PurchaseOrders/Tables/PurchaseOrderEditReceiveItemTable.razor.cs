using Shared.Commons;
using Shared.Enums.CurrencyEnums;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Responses;

namespace Web.Pages.PurchaseOrders.Tables;
public partial class PurchaseOrderEditReceiveItemTable
{



    [Parameter]
    public List<PurchaseOrderItemReceivedResponse> Items { get; set; } = new();
    public List<PurchaseOrderItemReceivedResponse> OrderedItems => Items.Count == 0 ? new() : Items.OrderBy(x => x.Order).ToList();
    [Parameter]
    public EventCallback<List<PurchaseOrderItemReceivedResponse>> ItemsChanged { get; set; }

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
    [Inject]
    public IRate _CurrencyService { get; set; } = null!;
    public ConversionRate RateList { get; set; } = null!;
    async Task ChangeDate(PurchaseOrderItemReceivedResponse item)
    {
        if (item.CurrencyDate == null)
        {
            return;
        }
        if (item.CurrencyDate.Value > DateTime.UtcNow)
        {
            _snackBar.ShowError($"Currency date must be less than Today: {DateTime.UtcNow.Date} ");
            return;
        }
        RateList = await _CurrencyService.GetRates(item.CurrencyDate.Value);
        var USDCOP = RateList == null ? 4000 : Math.Round(RateList.COP, 2);
        var USDEUR = RateList == null ? 1 : Math.Round(RateList.EUR, 2);
        item.USDCOP = USDCOP;
        item.USDEUR = USDEUR;
        StateHasChanged();

    }


}
