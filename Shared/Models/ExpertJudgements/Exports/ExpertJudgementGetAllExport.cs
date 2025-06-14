using Shared.Enums.ExportFiles;
using Shared.Models.ExpertJudgements.Responses;

namespace Shared.Models.ExpertJudgements.Exports
{
    public record ExpertJudgementGetAllExport(ExportFileType FileType, List<ExpertJudgementResponse> query);
}
