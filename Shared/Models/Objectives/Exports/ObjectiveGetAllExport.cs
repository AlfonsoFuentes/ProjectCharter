using Shared.Enums.ExportFiles;
using Shared.Models.Objectives.Responses;

namespace Shared.Models.Objectives.Exports
{
    public record ObjectiveGetAllExport(ExportFileType FileType, List<ObjectiveResponse> query);
}
