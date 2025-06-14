namespace Shared.Models.ExpendingTools.Responses
{
    public class ExpendingBudgetResponse
    {
        public override string ToString()
        {
            return $"{Name}";
        }
        public string Name { get; set; } = string.Empty;
        public string Nomenclatore { get; set; } = string.Empty;
        public string OrderNomenclatore => $"{Order}{Nomenclatore}";
        public int Order { get; set; } = 0;
        public double BudgetUSD { get; set; }
        public double GetBudgetByMonth(int month)
        {
            return Columns.Count == 0 ? 0 : Columns.Where(x => x.Month == month).Sum(x => x.Budget);
        }
        public List<ExpendingToolRow> Columns { get; set; } = new List<ExpendingToolRow>();
        public List<ExpendingToolRow> OrderedColumns => Columns.Count == 0 ? new() : Columns.OrderBy(x => x.Month).ToList();
        public double BudgetAssignedUSD => Columns.Count == 0 ? 0 : Columns.Sum(x => x.Budget);
        public double PendingBudgetUSD => BudgetUSD - BudgetAssignedUSD;


    }
}
