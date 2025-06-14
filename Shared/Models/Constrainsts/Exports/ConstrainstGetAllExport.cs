using Shared.Enums.ExportFiles;
using Shared.Models.Constrainsts.Responses;

namespace Shared.Models.Constrainsts.Exports
{
    public record ConstrainstGetAllExport(ExportFileType FileType, List<ConstrainstResponse> query);
}
