using Shared.Enums.ExportFiles;
using Shared.Models.BudgetItems.IndividualItems.Taxs.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.Taxs.Exports
{
    public record TaxGetAllExport(ExportFileType FileType, List<TaxResponse> query);
}
