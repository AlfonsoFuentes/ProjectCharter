using Shared.Enums.ExportFiles;
using Shared.Models.OtherTasks.Responses;

namespace Shared.Models.OtherTasks.Exports
{
    public record OtherTaskGetAllExport(ExportFileType FileType, List<OtherTaskResponse> query);
}
