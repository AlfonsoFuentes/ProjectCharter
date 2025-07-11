using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.BudgetItems.BasicEngineeringItems;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.PurchaseOrders.Responses;
using System.Text.Json.Serialization;

namespace Shared.Models.BudgetItemNewGanttTasks.Responses
{
    public class BudgetItemMonitoringNewGanttTaskResponse
    {
        BudgetItemWithPurchaseOrdersResponse? _BudgetItem { get; set; }
        [JsonIgnore]
        public BudgetItemWithPurchaseOrdersResponse? BudgetItem
        {
            get { return _BudgetItem; }
            set
            {
                _BudgetItem = value;

            }
        }
        [JsonIgnore]
        public string NomenclatoreName => BudgetItem == null ? string.Empty : BudgetItem.NomenclatoreName;
        [JsonIgnore]
        public string Nomenclatore => BudgetItem == null ? string.Empty : BudgetItem.Nomenclatore;
        [JsonIgnore]
        public string Name => BudgetItem == null ? string.Empty : BudgetItem.Name;

        public double BudgetUSD => BudgetItem == null ? 0 : BudgetItem.BudgetUSD;


        public Guid GanttTaskId { get; set; }
        public Guid BudgetItemId { get; set; }

        [JsonIgnore]
        public double TotalBudgetAssignedByOther => BudgetItem == null ? 0 : BudgetItem.BudgetItemGanttTasks
            .Where(x => x.GanttTaskId != GanttTaskId).Sum(x => x.BudgetAssignedUSD);
        [JsonIgnore]
        public double PendingToAssign => BudgetUSD - TotalBudgetAssignedByOther - BudgetPlannedUSD;

        public DateTime? PlannedStartDate => BudgetItem == null ? null : BudgetItem.PurchaseOrders.Min(x => x.ApprovedDate);
        public DateTime? MaxApprovedEndDate => BudgetItem == null ? null : BudgetItem.PurchaseOrders
            .Where(x => x.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Approved.Id
            || x.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Receiving.Id).Max(x => x.ExpectedDate);
        public DateTime? MaxClosedEndDate => BudgetItem == null ? null : BudgetItem.PurchaseOrders
            .Where(x => x.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Closed.Id).Max(x => x.ClosedDate);
        public DateTime? PlannedEndDate => MaxClosedEndDate > MaxApprovedEndDate ? MaxClosedEndDate : MaxApprovedEndDate;
        public double BudgetPlannedUSD { get; set; }
        [JsonIgnore]
        public double BudgetAssignedUSD => PurchaseOrders.Sum(x => x.TotalUSD);
        [JsonIgnore]
        public double BudgetAssignedActualUSD => PurchaseOrders.Sum(x => x.ActualPurchaseOrderUSD);
        [JsonIgnore]
        public double BudgetAssignedCommitmentUSD => PurchaseOrders.Sum(x => x.CommitmentPurchaseOrderUSD);
        [JsonIgnore]
        public double ToCommitUSD => BudgetPlannedUSD - BudgetAssignedUSD;
        public int Order { get; set; } = 0;
        //public Guid SelectedEngineeringItemsBudgetId { get; set; } = Guid.Empty;
        //public BasicResponse SelectedEngineeringItemsBudget { get; set; } = null!;
        [JsonIgnore]
        public List<PurchaseOrderResponse> PurchaseOrders =>
            _BudgetItem == null ? new List<PurchaseOrderResponse>() : _BudgetItem.PurchaseOrders;
            //SelectedEngineeringItemsBudgetId == Guid.Empty ? _BudgetItem.PurchaseOrders :
            //_BudgetItem.PurchaseOrders.Select(x => x).Where(y => y.PurchaseOrderItems.Any(z => z.BasicReponseId == SelectedEngineeringItemsBudgetId)).ToList();
    }

}
