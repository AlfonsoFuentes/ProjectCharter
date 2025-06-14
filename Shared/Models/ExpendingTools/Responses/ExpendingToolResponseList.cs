using Shared.Models.FileResults.Generics.Reponses;
using System.Collections.Generic;

namespace Shared.Models.ExpendingTools.Responses
{
    public class ExpendingToolResponseList : IResponseAll
    {
        public List<ExpendingBudgetResponse> ExpensesItems { get; set; } = new List<ExpendingBudgetResponse>();
        public List<ExpendingBudgetResponse> CapitalItems { get; set; } = new List<ExpendingBudgetResponse>();
        public List<ExpendingBudgetResponse> EngineeringItems { get; set; } = new List<ExpendingBudgetResponse>();
        public List<ExpendingBudgetResponse> Items => [.. ExpensesItems,..CapitalItems, ..EngineeringItems];
        public List<ExpendingBudgetResponse> OrderedItems => Items.Count == 0 ? new() :
            Items.OrderBy(x => x.Order).ThenBy(x => x.Nomenclatore).ToList();
        public double BudgetUSD => Items.Count == 0 ? 0 : Items.Sum(x => x.BudgetUSD);
        public double PendingBudgetUSD => Items.Count == 0 ? 0 : Items.Sum(x => x.PendingBudgetUSD);
        public List<ExpendingToolRow> Columns => OrderedItems.Count == 0 ? new() : getColumns();
        public List<ExpendingToolRow> OrderedColumns => Columns.Count == 0 ? new() : Columns.OrderBy(x => x.Order).ToList();
        List<ExpendingToolRow> getColumns()
        {
            List<ExpendingToolRow> result = new();
            foreach (var column in OrderedItems[0].Columns)
            {
                double budget = OrderedItems.Select(x => x.Columns.Where(y => y.Month == column.Month)?.Sum(x => x.Budget) ?? 0).Sum();
                ExpendingToolRow row = new ExpendingToolRow(column.Order,column.Month, column.ColumnName, budget);
                result.Add(row);
            }
            return result;
        }
    }
}
