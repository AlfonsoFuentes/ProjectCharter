using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.PurchaseOrders.Records;
using Shared.Models.PurchaseOrders.Responses;

namespace MudBlazorWeb.Pages.PurchaseOrders.Tables;
public partial class PurchaseOrderCreatedTable
{
    public List<PurchaseOrderResponse> Items { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        await GetAll();
    }

    async Task GetAll()
    {

        var result = await GenericService.GetAll<PurchaseOrderResponseList, PurchaseOrderGetAll>(new PurchaseOrderGetAll()
        {
            Status = PurchaseOrderStatusEnum.Created,
        });
        if (result.Succeeded)
        {

            Items = result.Data.Items;
        }
    }
}
