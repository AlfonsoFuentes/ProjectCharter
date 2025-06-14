using Shared.Enums.ExportFiles;
using Shared.Models.MeetingAgreements.Responses;

namespace Shared.Models.MeetingAgreements.Exports
{
    public record MeetingAgreementGetAllExport(ExportFileType FileType, List<MeetingAgreementResponse> query);
}
