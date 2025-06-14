using Shared.Enums.ExportFiles;
using Shared.Models.MeetingAttendants.Responses;

namespace Shared.Models.MeetingAttendants.Exports
{
    public record MeetingAttendantGetAllExport(ExportFileType FileType, List<MeetingAttendantResponse> query);
}
