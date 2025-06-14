using Shared.Enums.ExportFiles;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.BudgetItems.Exports
{
    public class BudgetItemExportGetAll : IGetAll
    {
        public string EndPointName => StaticClass.BudgetItems.EndPoint.Export;
        public string Name { get; set; } = string.Empty;
        public List<BudgetItemExport> Items { get; set; } = new();
        public ExportFileType ExportFile { get; set; } = ExportFileType.Excel;
    }

}