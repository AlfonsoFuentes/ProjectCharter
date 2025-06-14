using Shared.Enums.ExportFiles;
using Shared.Models.Templates.Instruments.Responses;

namespace Shared.Models.Templates.Instruments.Exports
{
    public record InstrumentTemplateGetAllExport(ExportFileType FileType, List<InstrumentTemplateResponse> query);
}
