using Shared.Enums.ExportFiles;
using Shared.Models.MonitoringLogs.Responses;

namespace Shared.Models.MonitoringLogs.Exports
{
    public record MonitoringLogGetAllExport(ExportFileType FileType, List<MonitoringLogResponse> query);
}
