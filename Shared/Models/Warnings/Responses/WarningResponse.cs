using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.MonitoringTask.Responses;
using Shared.Models.PurchaseOrders.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Warnings.Responses
{
    public class WarningResponse
    {
        public PurchaseOrderResponse? PurchaseOrder { get; set; }
        public MonitoringGanttTaskResponse? GanttTask { get; set; } = null!;
        public string WarningText { get; set; }=string.Empty;   
    }
}
