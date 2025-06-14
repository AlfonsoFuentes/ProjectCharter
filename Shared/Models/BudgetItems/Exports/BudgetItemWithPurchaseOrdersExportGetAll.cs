using Shared.Enums.ExportFiles;
using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.BudgetItems.Exports
{
    public class BudgetItemWithPurchaseOrdersExportGetAll : IGetAll
    {
        public string EndPointName => StaticClass.BudgetItems.EndPoint.ExportWithPurchaseOrders;
        public string Name { get; set; } = string.Empty;
        public List<BudgetItemWithPurchaseordersExport> Items { get; set; } = new();
        public ExportFileType ExportFile { get; set; } = ExportFileType.Excel;
    }

}