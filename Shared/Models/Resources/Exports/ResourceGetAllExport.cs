using Shared.Enums.ExportFiles;
using Shared.Models.Resources.Responses;

namespace Shared.Models.Resources.Exports
{
    public record ResourceGetAllExport(ExportFileType FileType, List<ResourceResponse> query);
}
