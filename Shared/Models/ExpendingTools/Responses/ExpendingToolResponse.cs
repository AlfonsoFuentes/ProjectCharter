namespace Shared.Models.ExpendingTools.Responses
{
    public class ExpendingToolRow
    {
        public override string ToString()
        {
            return $"{ColumnName} {Budget}";
        }
        public ExpendingToolRow(int Order,int Month, string ColumnName, double Budget)
        {
            this.Order = Order;
            this.Month = Month;
            this.ColumnName = ColumnName;
            this.Budget = Budget;
        }
        public int Order { get; set; } = 0;
        public int Month { get; set; } = 0;
        public string ColumnName { get; set; } = string.Empty;
        public double Budget { get; set; } = 0;
    }
}
