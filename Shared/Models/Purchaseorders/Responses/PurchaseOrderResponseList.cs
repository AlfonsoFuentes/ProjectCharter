using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class PurchaseOrderResponseList: IResponseAll
    {
        public List<PurchaseOrderResponse> Items { get; set; } = new();
    }
}
