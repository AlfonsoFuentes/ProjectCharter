namespace Shared.Models.MonitoringExpendingTools.Responses
{
    public class BudgetItemGantMonitoringReportDto
    {
        public override string ToString() => $"{TaskName} - {BudgetItemName} (Planned: {BudgetPlannedUSD:C} (Assigned: {AssignedUSD:C} (Pending: {TaskPendingBudgetUSD:C})";
        public Guid BudgetItemId { get; set; } = Guid.Empty;
        //public Guid? BasicEngineeringItemId { get; set; } = null;
        public Guid GanttTaskId { get; set; } = Guid.Empty;
        public double BudgetPlannedUSD { get; set; } = 0;
        public double AssignedUSD { get; set; } = 0;
        public DateTime EndDate { get; set; } = DateTime.MinValue;
        public double TaskPendingBudgetUSD => BudgetPlannedUSD - AssignedUSD > 0 ? BudgetPlannedUSD - AssignedUSD : 0;
        public string TaskName { get; set; } = string.Empty;
        public string BudgetItemName { get; set; } = string.Empty;
    }
}
