using Shared.Models.BudgetItems.Exports;
namespace Server.EndPoint.BudgetItems.Exports
{
    public static class GetAllBudgetItemExportEndPoint
    {
        public class EndPointBudgetItems : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BudgetItems.EndPoint.Export, (BudgetItemExportGetAll respose) =>
                {
                    var query = respose.Items.AsQueryable();
                    var response = respose.ExportFile == ExportFileType.pdf ?
                  ExportPDFExtension.ExportPDF(query, $"Budget Items {respose.Name}", $"{respose.Name}") :
                  ExportExcelExtension.ToExcel(query, $"Budget Items {respose.Name}", "List");


                    return Result<FileResult>.Success(response);
                });
            }




        }
        public class EndPointBudgetItemWithPurchaseOrders : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BudgetItems.EndPoint.ExportWithPurchaseOrders, (BudgetItemWithPurchaseOrdersExportGetAll respose) =>
                {
                    var query = respose.Items.AsQueryable();
                    var response = respose.ExportFile == ExportFileType.pdf ?
                  ExportPDFExtension.ExportPDF(query, $"Expenses tool {respose.Name}", $"{respose.Name}") :
                  ExportExcelExtension.ToExcel(query, $"Expenses tool {respose.Name}", "List");


                    return Result<FileResult>.Success(response);
                });
            }




        }
    }
}