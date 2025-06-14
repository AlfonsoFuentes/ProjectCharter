using Shared.Enums.ExportFiles;
using Shared.Models.Acquisitions.Responses;

namespace Shared.Models.Acquisitions.Exports
{
    public record AcquisitionGetAllExport(ExportFileType FileType, List<AcquisitionResponse> query);
}
