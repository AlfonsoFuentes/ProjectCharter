using Shared.Models.BudgetItemNewGanttTasks.Responses;
using Shared.Models.BudgetItems.BasicEngineeringItems;
using System.Text.Json.Serialization;

namespace Shared.Models.BudgetItems.Responses
{
    public class BudgetItemResponse : BaseResponse, IBudgetItemResponse
    {
        public override string ToString()
        {
            return Name;
        }
        public virtual double BudgetUSD { get; set; }
        public int OrderList { get; set; }
        public string Nomenclatore { get; set; } = string.Empty;
        public string NomenclatoreName => $"{Nomenclatore}-{Name}";
     
        public virtual bool IsAlteration { get; set; }
        public bool IsTaxes { get; set; }
        public List<BudgetItemNewGanttTaskResponse> BudgetItemGanttTasks { get; set; } = new();
        [JsonIgnore]
        public double BudgetAssigned => BudgetItemGanttTasks.Sum(x => x.BudgetAssignedUSD);
        [JsonIgnore]
        public bool IsAvailableToAssignedToTask => BudgetUSD - BudgetAssigned > 0;

        public virtual List<BasicResponse> BasicEngineeringItems { get; set; } = new List<BasicResponse>();     
      
        public bool HasSubItems=> BasicEngineeringItems.Any();


    }

}
