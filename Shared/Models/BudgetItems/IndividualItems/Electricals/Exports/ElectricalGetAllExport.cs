using Shared.Enums.ExportFiles;
using Shared.Models.BudgetItems.IndividualItems.Electricals.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.Electricals.Exports
{
    public record ElectricalGetAllExport(ExportFileType FileType, List<ElectricalResponse> query);
}
