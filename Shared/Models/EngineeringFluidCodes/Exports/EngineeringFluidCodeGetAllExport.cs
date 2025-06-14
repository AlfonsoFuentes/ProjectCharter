using Shared.Enums.ExportFiles;
using Shared.Models.EngineeringFluidCodes.Responses;

namespace Shared.Models.EngineeringFluidCodes.Exports
{
    public record EngineeringFluidCodeGetAllExport(ExportFileType FileType, List<EngineeringFluidCodeResponse> query);
}
