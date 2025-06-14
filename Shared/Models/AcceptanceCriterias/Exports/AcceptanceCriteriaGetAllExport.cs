using Shared.Enums.ExportFiles;
using Shared.Models.AcceptanceCriterias.Responses;

namespace Shared.Models.AcceptanceCriterias.Exports
{
    public record AcceptanceCriteriaGetAllExport(ExportFileType FileType, List<AcceptanceCriteriaResponse> query);
}
