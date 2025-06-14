using Shared.Enums.ExportFiles;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.Pipes.Exports
{
    public record PipeGetAllExport(ExportFileType FileType, List<PipeResponse> query);
}
