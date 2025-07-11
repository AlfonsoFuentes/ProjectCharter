using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Entities.BudgetItems
{
    public class BudgetItemNewGanttTask : AuditableEntity<Guid>, ITenantEntity
    {

        public string TenantId { get; set; } = string.Empty;
        public BudgetItem BudgetItem { get; set; } = null!;
        public Guid BudgetItemId { get; set; }
        public NewGanttTask NewGanttTask { get; set; } = null!;
        public Guid NewGanttTaskId { get; set; }

        public double GanttTaskBudgetAssigned { get; set; }

        public static BudgetItemNewGanttTask Create(Guid _BudgetItemId, Guid _NewGanttTaskId)
        {
            return new BudgetItemNewGanttTask()
            {
                Id = Guid.NewGuid(),
                BudgetItemId = _BudgetItemId,
                NewGanttTaskId = _NewGanttTaskId,



            };
        }

        //public BasicEngineeringItem? SelectedBasicEngineeringItem { get; set; } = null!;
        //public Guid? SelectedBasicEngineeringItemId { get; set; }

        //public double ToCommitUSD => SelectedBasicEngineeringItem == null ? BudgetItem == null ? 0 : BudgetItem.ToCommitUSD : SelectedBasicEngineeringItem.ToCommitUSD;

    }

}
