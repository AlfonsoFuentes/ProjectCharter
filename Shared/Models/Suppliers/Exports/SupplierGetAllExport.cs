using Shared.Enums.ExportFiles;
using Shared.Models.Suppliers.Responses;

namespace Shared.Models.Suppliers.Exports
{
    public record SupplierGetAllExport(ExportFileType FileType, List<SupplierResponse> query);
}
