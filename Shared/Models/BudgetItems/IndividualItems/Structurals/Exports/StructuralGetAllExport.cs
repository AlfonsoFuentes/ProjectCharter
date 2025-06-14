using Shared.Enums.ExportFiles;
using Shared.Models.BudgetItems.IndividualItems.Structurals.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.Structurals.Exports
{
    public record StructuralGetAllExport(ExportFileType FileType, List<StructuralResponse> query);
}
