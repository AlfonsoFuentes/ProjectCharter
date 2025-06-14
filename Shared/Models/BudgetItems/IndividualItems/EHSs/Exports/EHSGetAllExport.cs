using Shared.Enums.ExportFiles;
using Shared.Models.BudgetItems.IndividualItems.EHSs.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.EHSs.Exports
{
    public record EHSGetAllExport(ExportFileType FileType, List<EHSResponse> query);
}
