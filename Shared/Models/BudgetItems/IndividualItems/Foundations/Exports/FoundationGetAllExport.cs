using Shared.Enums.ExportFiles;
using Shared.Models.BudgetItems.IndividualItems.Foundations.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.Foundations.Exports
{
    public record FoundationGetAllExport(ExportFileType FileType, List<FoundationResponse> query);
}
