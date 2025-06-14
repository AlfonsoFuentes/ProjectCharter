using Shared.Enums.ExportFiles;
using Shared.Models.KnownRisks.Responses;

namespace Shared.Models.KnownRisks.Exports
{
    public record KnownRiskGetAllExport(ExportFileType FileType, List<KnownRiskResponse> query);
}
