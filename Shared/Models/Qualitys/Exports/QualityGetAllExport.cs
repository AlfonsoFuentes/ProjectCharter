using Shared.Enums.ExportFiles;
using Shared.Models.Qualitys.Responses;

namespace Shared.Models.Qualitys.Exports
{
    public record QualityGetAllExport(ExportFileType FileType, List<QualityResponse> query);
}
