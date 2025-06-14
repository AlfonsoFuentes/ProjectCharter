using Shared.Enums.ExportFiles;
using Shared.Models.BudgetItems.IndividualItems.Valves.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.Valves.Exports
{
    public record ValveGetAllExport(ExportFileType FileType, List<ValveResponse> query);
}
