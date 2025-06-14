using Shared.Enums.ExportFiles;
using Shared.Models.Templates.Valves.Responses;

namespace Shared.Models.Templates.Valves.Exports
{
    public record ValveTemplateGetAllExport(ExportFileType FileType, List<ValveTemplateResponse> query);
}
