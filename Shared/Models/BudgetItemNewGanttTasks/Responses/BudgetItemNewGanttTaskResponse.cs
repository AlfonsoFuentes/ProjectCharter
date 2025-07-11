using Shared.Models.BudgetItems.BasicEngineeringItems;
using Shared.Models.BudgetItems.Responses;
using System.Text.Json.Serialization;

namespace Shared.Models.BudgetItemNewGanttTasks.Responses
{
    public class BudgetItemNewGanttTaskResponse
    {
        BudgetItemResponse? _BudgetItem { get; set; }
        [JsonIgnore]
        public BudgetItemResponse? BudgetItem
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
        //[JsonIgnore]
        //public bool HasSubItems => BudgetItem == null ? false : BudgetItem.HasSubItems;
        public double BudgetUSD => BudgetItem == null ? 0 : BudgetItem.BudgetUSD;// BudgetItem.HasSubItems ? SelectedEngineeringItemsBudget == null ? 0 : SelectedEngineeringItemsBudget.BudgetUSD : BudgetItem.BudgetUSD;


        public Guid GanttTaskId { get; set; }
        public Guid BudgetItemId { get; set; }

        [JsonIgnore]
        public double TotalBudgetAssignedByOther => BudgetItem == null ? 0 : BudgetItem.BudgetItemGanttTasks.Where(x => x.GanttTaskId != GanttTaskId).Sum(x => x.BudgetAssignedUSD);
        //    BudgetItem.BudgetItemGanttTasks.Where(x => x.GanttTaskId != GanttTaskId).Sum(x => x.BudgetAssignedUSD) :
        //     BudgetItem.BudgetItemGanttTasks.Where(x => x.GanttTaskId != GanttTaskId && x.SelectedEngineeringItemsBudgetId == SelectedEngineeringItemsBudget.Id).Sum(x => x.BudgetAssignedUSD);
        [JsonIgnore]
        public double PendingToAssign => BudgetUSD - TotalBudgetAssignedByOther - BudgetAssignedUSD;


        public double BudgetAssignedUSD { get; set; }
        public int Order { get; set; } = 0;

        //public Guid? SelectedEngineeringItemsBudgetId { get; set; }
        //public BasicResponse SelectedEngineeringItemsBudget { get; set; } = null!;
        //[JsonIgnore]
        //public string BasicResponseName => SelectedEngineeringItemsBudget == null ? string.Empty : SelectedEngineeringItemsBudget.Name;

    }

}
