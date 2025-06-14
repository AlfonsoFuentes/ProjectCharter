using Shared.Models.PurchaseOrders.Responses;

namespace Shared.Models.BudgetItems.Responses
{
    public class BudgetItemWithPurchaseOrdersResponse : BudgetItemResponse, IBudgetItemWithPurchaseOrderResponse
    {
            
        public Guid ProjectId { get; set; }
      
       
        public List<PurchaseOrderResponse> PurchaseOrders { get; set; } = new();
        public double ActualUSD { get; set; }
        public double CommitmentUSD { get; set; }
        public double PotentialUSD { get; set; }
      
        public double AssignedUSD => ActualUSD + CommitmentUSD + PotentialUSD;
        public double ToCommitUSD => BudgetUSD - AssignedUSD;

      

       
    }
}
