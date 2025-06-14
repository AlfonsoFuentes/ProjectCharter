using Shared.Enums.ExportFiles;
using Shared.Models.BudgetItems.IndividualItems.EngineeringDesigns.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.EngineeringDesigns.Exports
{
    public record EngineeringDesignGetAllExport(ExportFileType FileType, List<EngineeringDesignResponse> query);
}
