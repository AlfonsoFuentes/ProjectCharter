using Shared.Enums.ExportFiles;
using Shared.Models.PurchaseOrders.Responses;

namespace Shared.Models.PurchaseOrders.Exports
{
    public record PurchaseOrderGetAllExport(ExportFileType FileType, List<PurchaseOrderResponse> query);
}
