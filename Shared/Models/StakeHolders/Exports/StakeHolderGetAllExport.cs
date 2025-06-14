using Shared.Enums.ExportFiles;
using Shared.Models.StakeHolders.Responses;

namespace Shared.Models.StakeHolders.Exports
{
    public record StakeHolderGetAllExport(ExportFileType FileType, List<StakeHolderResponse> query);
}
