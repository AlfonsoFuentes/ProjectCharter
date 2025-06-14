using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.BudgetItems.Records
{
    public class BudgetItemWithPurchaseOrderGetById : IGetById
    {

        public string EndPointName => StaticClass.BudgetItems.EndPoint.GetWithPurchaseorderById;
        public Guid Id { get; set; }

    }
}
