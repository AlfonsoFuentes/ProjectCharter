using Shared.Enums.ExportFiles;
using Shared.Models.Scopes.Responses;

namespace Shared.Models.Scopes.Exports
{
    public record ScopeGetAllExport(ExportFileType FileType, List<ScopeResponse> query);
}
