using Shared.Enums.ExportFiles;
using Shared.Models.BudgetItems.IndividualItems.Paintings.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.Paintings.Exports
{
    public record PaintingGetAllExport(ExportFileType FileType, List<PaintingResponse> query);
}
