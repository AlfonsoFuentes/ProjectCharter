namespace Shared.Models.MonitoringExpendingTools.Responses
{
    public class ProjectMonitoringReportDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string ProjectNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CECName => $"CEC0000{ProjectNumber}";
        public List<MonthlyMonitoringData> MonthlyData { get; set; } = new();
        public double BudgetUSD { get; set; } = 0;
        public double PendingBudgetUSD => BudgetUSD - BudgetAssignedUSD;
        public double BudgetAssignedUSD => MonthlyData.Count == 0 ? 0 : MonthlyData.Sum(x => x.ValueUSD);
        public List<BudgetItemGantMonitoringReportDto> GanttItems { get; set; } = new();


    }
}
