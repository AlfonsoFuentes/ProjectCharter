using Shared.Enums.ExportFiles;
using Shared.Models.Meetings.Responses;

namespace Shared.Models.Meetings.Exports
{
    public record MeetingGetAllExport(ExportFileType FileType, List<MeetingResponse> query);
}
