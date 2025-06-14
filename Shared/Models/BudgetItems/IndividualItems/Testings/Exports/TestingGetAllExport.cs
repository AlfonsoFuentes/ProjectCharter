using Shared.Enums.ExportFiles;
using Shared.Models.BudgetItems.IndividualItems.Testings.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.Testings.Exports
{
    public record TestingGetAllExport(ExportFileType FileType, List<TestingResponse> query);
}
