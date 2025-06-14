using Shared.Models.ExpendingTools.Responses;
using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.MonitoringExpendingTools.Responses
{
    public class BudgetItemMonitoringReportDtoList : IResponseAll
    {
        public List<BudgetItemMonitoringReportDto> Items { get; set; } = new List<BudgetItemMonitoringReportDto>();

        public double BudgetUSD => Items.Count == 0 ? 0 : Items.Sum(x => x.BudgetUSD);
        public double PendingBudgetUSD => Items.Count == 0 ? 0 : Items.Sum(x => x.PendingBudgetUSD);
        public List<BudgetItemMonitoringReportDto> OrderedItems => Items.Count == 0 ? new() :
           Items.OrderBy(x => x.Order).ThenBy(x => x.Nomenclatore).ToList();
        public List<ColumnMonitoringName> Columns => OrderedItems.Count == 0 ? new() : OrderedItems[0].MonthlyData.Select(x => new ColumnMonitoringName()
        {
            Order = x.Order,
            Name = x.ColumnName,
            ValueUSD = OrderedItems.Sum(y => y.MonthlyData.Where(z => z.Order == x.Order).Sum(z => z.ValueUSD))
        }).ToList();
        public List<ColumnMonitoringName> OrderedColumns => Columns.Count == 0 ? new() : Columns.OrderBy(x => x.Order).ToList();
        public List<BudgetItemGantMonitoringReportDto> GanttItems => Items.Count == 0 ? new() :Items.SelectMany(x => x.GanttItems).ToList();

    }
    public class BudgetItemMonitoringReportDto
    {
        public int Order { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Nomenclatore { get; set; } = string.Empty;
        public List<MonthlyMonitoringData> MonthlyData { get; set; } = new();
        public double BudgetUSD { get; set; } = 0;
        public double PendingBudgetUSD => BudgetUSD - BudgetAssignedUSD;
        public double BudgetAssignedUSD => MonthlyData.Count == 0 ? 0 : MonthlyData.Sum(x => x.ValueUSD);

        public List<BudgetItemGantMonitoringReportDto> GanttItems { get; set; } = new();


    }
    public class BudgetItemGantMonitoringReportDto
    {
        public override string ToString() => $"{TaskName} - {BudgetItemName} (Planned: {BudgetPlannedUSD:C} (Assigned: {AssignedUSD:C} (Pending: {TaskPendingBudgetUSD:C})";
        public Guid BudgetItemId { get; set; } = Guid.Empty;
        public Guid? BasicEngineeringItemId { get; set; } = null;
        public Guid GanttTaskId { get; set; } = Guid.Empty;
        public double BudgetPlannedUSD { get; set; } = 0;
        public double AssignedUSD { get; set; } = 0;
        public DateTime EndDate { get; set; } = DateTime.MinValue;
        public double TaskPendingBudgetUSD => BudgetPlannedUSD - AssignedUSD > 0 ? BudgetPlannedUSD - AssignedUSD : 0;
        public string TaskName { get; set; } = string.Empty;
        public string BudgetItemName { get; set; } = string.Empty;
    }

    public class MonthlyMonitoringData
    {
        public string ColumnName { get; set; } = string.Empty;
        public int Order { get; set; }
        public double ValueUSD { get; set; } = 0;
        //public double ValueByPOUSD { get; set; } = 0;


    }
    public class ColumnMonitoringName
    {
        public int Order { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public double ValueUSD { get; set; } = 0;
    }
}
