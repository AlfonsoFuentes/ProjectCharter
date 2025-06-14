using Shared.Enums.ExportFiles;
using Shared.Models.BudgetItems.IndividualItems.Equipments.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.Equipments.Exports
{
    public record EquipmentGetAllExport(ExportFileType FileType, List<EquipmentResponse> query);
}
