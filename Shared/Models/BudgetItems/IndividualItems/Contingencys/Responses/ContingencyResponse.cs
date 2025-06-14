using Shared.Models.BudgetItems.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.Contingencys.Responses
{
    public class ContingencyResponse : BudgetItemWithPurchaseOrdersResponse
    {

        public double Percentage { get; set; }
    }
}
