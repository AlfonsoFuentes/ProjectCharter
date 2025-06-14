using Shared.Enums.ExportFiles;
using Shared.Models.Brands.Responses;

namespace Shared.Models.Brands.Exports
{
    public record BrandGetAllExport(ExportFileType FileType, List<BrandResponse> query);
}
