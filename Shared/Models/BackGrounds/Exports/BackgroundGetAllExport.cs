using Shared.Enums.ExportFiles;
using Shared.Models.Backgrounds.Responses;

namespace Shared.Models.Backgrounds.Exports
{
    public record BackgroundGetAllExport(ExportFileType FileType, List<BackGroundResponse> query);
}
