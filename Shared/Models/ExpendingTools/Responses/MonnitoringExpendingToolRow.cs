namespace Shared.Models.ExpendingTools.Responses
{
    public class MonnitoringExpendingToolRow
    {
        public override string ToString()
        {
            return $"{ColumnName} {Budget}";
        }
        public MonnitoringExpendingToolRow(int Order,int Month, string ColumnName, double Budget=0, double Planned = 0)
        {
            this.Month = Month;
            this.ColumnName = ColumnName;
            this.Budget = Budget;
            this.Planned = Planned;
        }
        public int Order { get; set; } = 0;
        public int Month { get; set; } = 0;
        public string ColumnName { get; set; } = string.Empty;
        public double Budget { get; set; } = 0;
        public double Planned { get; set; } = 0;
    }
}
