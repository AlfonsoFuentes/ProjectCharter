using Shared.Models.PurchaseOrders.Responses;

namespace Shared.Models.MonitoringExpendingTools.Responses
{
    public class MonthlyMonitoringData
    {
        public string ColumnName { get; set; } = string.Empty;
        public int Order { get; set; }
        public double ValueUSD { get; set; } = 0;
        public int Month { get; set; } = 0; // 1-12 for January to December
        public int Year { get; set; }

        public List<PurchaseOrderResponse> Actuals { get; set; } = new List<PurchaseOrderResponse>();
        public List<PurchaseOrderResponse> Commitmments { get; set; } = new List<PurchaseOrderResponse>();
    }
}
