using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.PurchaseOrders.Records;
using Shared.Models.PurchaseOrders.Responses;
using static MudBlazor.CategoryTypes;

namespace Web.Pages.PurchaseOrders.Tables;
public partial class PurchaseOrderApprovedTable
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
            Status = PurchaseOrderStatusEnum.Approved,
        });
        if (result.Succeeded)
        {

            Items = result.Data.Items;
        }
    }
}
