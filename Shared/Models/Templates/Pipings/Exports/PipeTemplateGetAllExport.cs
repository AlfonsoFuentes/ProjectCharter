using Shared.Enums.ExportFiles;
using Shared.Models.Templates.Pipings.Responses;

namespace Shared.Models.Templates.Pipings.Exports
{
    public record PipeTemplateGetAllExport(ExportFileType FileType, List<PipeTemplateResponse> query);
}
