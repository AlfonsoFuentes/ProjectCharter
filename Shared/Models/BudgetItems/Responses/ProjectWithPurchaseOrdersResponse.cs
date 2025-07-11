using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.PurchaseOrders.Responses;

namespace Shared.Models.BudgetItems.Responses
{
    public class ProjectWithPurchaseOrdersResponse : IResponse
    {

        public Guid ProjectId { get; set; }


        public List<PurchaseOrderResponse> PurchaseOrders { get; set; } = new();
   
        public Guid Id { get; set; }
        public string Name { get; set; }=string.Empty;
    }
}
