using Shared.Enums.ExportFiles;
using Shared.Models.Communications.Responses;

namespace Shared.Models.Communications.Exports
{
    public record CommunicationGetAllExport(ExportFileType FileType, List<CommunicationResponse> query);
}
