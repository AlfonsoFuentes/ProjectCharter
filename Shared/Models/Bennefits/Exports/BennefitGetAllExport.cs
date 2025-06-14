using Shared.Enums.ExportFiles;
using Shared.Models.Bennefits.Responses;

namespace Shared.Models.Bennefits.Exports
{
    public record BennefitGetAllExport(ExportFileType FileType, List<BennefitResponse> query);
}
