using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.MonitoringExpendingTools.Responses;

namespace Shared.Models.ExpendingTools.Responses
{
   
    public class BudgetItemReportDtoList : IResponseAll
    {
        public List<BudgetItemReportDto> Items { get; set; } = new List<BudgetItemReportDto>();

        public double BudgetUSD => Items.Count == 0 ? 0 : Items.Sum(x => x.BudgetUSD);
        public double PendingBudgetUSD => Items.Count == 0 ? 0 : Items.Sum(x => x.PendingBudgetUSD);
        public List<BudgetItemReportDto> OrderedItems => Items.Count == 0 ? new() :
           Items.OrderBy(x => x.Order).ThenBy(x => x.Nomenclatore).ToList();
        public List<ColumnName> Columns => OrderedItems.Count == 0 ? new() : OrderedItems[0].MonthlyData.Select(x => new ColumnName()
        {
            Order = x.Order,
            Name = x.ColumnName,
          
            BudgetUSD = OrderedItems.Sum(y => y.MonthlyData.Where(z => z.Order == x.Order).Sum(z => z.BudgetUSD))
        }).ToList();
        public List<ColumnName> OrderedColumns => Columns.Count == 0 ? new() : Columns.OrderBy(x => x.Order).ToList();

    }
    public class BudgetItemReportDto
    {
        public int Order { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Nomenclatore { get; set; } = string.Empty;
        public List<MonthlyData> MonthlyData { get; set; } = new();
        public double BudgetUSD { get; set; } = 0;
        public double PendingBudgetUSD => BudgetUSD - BudgetAssignedUSD;
        public double BudgetAssignedUSD => MonthlyData.Count == 0 ? 0 : MonthlyData.Sum(x => x.BudgetUSD);
       
    }

    public class MonthlyData
    {
        public string ColumnName { get; set; } = string.Empty;
        public int Order { get; set; }
   
        public double BudgetUSD { get; set; } = 0;
       

    }
    public class ColumnName
    {
        public int Order { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
     
        public double BudgetUSD { get; set; } = 0;
    }
   

}
