using Shared.Enums.ExportFiles;
using Shared.Models.BudgetItems.IndividualItems.Instruments.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.Instruments.Exports
{
    public record InstrumentGetAllExport(ExportFileType FileType, List<InstrumentResponse> query);
}
