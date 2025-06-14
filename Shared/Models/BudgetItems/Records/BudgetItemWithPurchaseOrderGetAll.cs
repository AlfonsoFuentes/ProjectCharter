using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.BudgetItems.Records
{
    public class BudgetItemWithPurchaseOrderGetAll : IGetAll
    {

        public string EndPointName => StaticClass.BudgetItems.EndPoint.GetAllWithPurchaseorder;
        public Guid ProjectId { get; set; }

    }
}
