using Shared.Enums.ExportFiles;
using Shared.Models.Requirements.Responses;

namespace Shared.Models.Requirements.Exports
{
    public record RequirementGetAllExport(ExportFileType FileType, List<RequirementResponse> query);
}
