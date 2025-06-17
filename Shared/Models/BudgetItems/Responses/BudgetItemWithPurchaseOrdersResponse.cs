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
        public double _ActualUSD => PurchaseOrders.Sum(x => x.ActualPurchaseOrderUSD);
        public double _CommitmentUSD => PurchaseOrders.Sum(x => x.CommitmentPurchaseOrderUSD);
        public double _PotentialUSD => PurchaseOrders.Sum(x => x.PotentialPurchaseOrderUSD);

        public double AssignedUSD => _ActualUSD + _CommitmentUSD + _PotentialUSD;
        public double ToCommitUSD => BudgetUSD - AssignedUSD;

      

       
    }
}
