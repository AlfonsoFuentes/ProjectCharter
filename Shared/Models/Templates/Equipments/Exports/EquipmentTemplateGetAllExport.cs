using Shared.Enums.ExportFiles;
using Shared.Models.Templates.Equipments.Responses;

namespace Shared.Models.Templates.Equipments.Exports
{
    public record EquipmentTemplateGetAllExport(ExportFileType FileType, List<EquipmentTemplateResponse> query);
}
