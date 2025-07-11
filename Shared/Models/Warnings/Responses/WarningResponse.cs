using Shared.Models.MonitoringTask.Responses;
using Shared.Models.PurchaseOrders.Responses;

namespace Shared.Models.Warnings.Responses
{
    public class WarningResponse
    {
        public PurchaseOrderResponse? PurchaseOrder { get; set; }
        public MonitoringGanttTaskResponse? GanttTask { get; set; } = null!;
        public string WarningText { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectNumber { get; set; } = string.Empty;

        public int WarningType =>
            PurchaseOrder != null ? 1 :
            GanttTask != null ? 2 : 0;
    }
}
