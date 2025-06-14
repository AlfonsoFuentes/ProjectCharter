using Shared.Enums.ExportFiles;
using Shared.Models.Assumptions.Responses;

namespace Shared.Models.Assumptions.Exports
{
    public record AssumptionGetAllExport(ExportFileType FileType, List<AssumptionResponse> query);
}
