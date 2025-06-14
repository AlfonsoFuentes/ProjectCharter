using Shared.Enums.ExportFiles;
using Shared.Models.BudgetItems.IndividualItems.Alterations.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.Alterations.Exports
{
    public record AlterationGetAllExport(ExportFileType FileType, List<AlterationResponse> query);
}
