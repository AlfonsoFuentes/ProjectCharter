using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.BudgetItems.Records
{
    public class BudgetItemWithPurchaseOrderGetByIdAndDate : IGetById
    {

        public string EndPointName => StaticClass.BudgetItems.EndPoint.GetWithPurchaseorderByIdAndDate;
        public Guid Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public Guid ProjectId { get; set; } = Guid.Empty;

    }
}
