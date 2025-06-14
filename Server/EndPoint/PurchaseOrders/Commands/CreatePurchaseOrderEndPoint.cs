using Server.Database.Entities.PurchaseOrders;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.PurchaseOrders.Requests;

namespace Server.EndPoint.PurchaseOrders.Commands
{

    public static class CreatePurchaseOrderEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PurchaseOrders.EndPoint.Create, async (CreatePurchaseOrderRequest Data, IRepository Repository) =>
                {

                    var row = PurchaseOrder.Create(Data.ProjectId);


                    await Repository.AddAsync(row);
                    Data.Map(row);
                    foreach (var item in Data.SelectedPurchaseOrderItems)
                    {
                        var rowitem = PurchaseOrderItem.Create(row.Id, item.BudgetItemId);
                        rowitem.Name = item.Name;
                        rowitem.UnitaryValueQuoteCurrency = item.UnitaryQuoteCurrency;
                        rowitem.Quantity = item.Quantity;
                        rowitem.Order = item.Order;
                        if(item.BasicResponse != null)
                        {
                            rowitem.BasicEngineeringItemId = item.BasicResponse.Id;
                        
                        }

                        await Repository.AddAsync(rowitem);
                    }

                    List<string> cache = [.. StaticClass.PurchaseOrders.Cache.KeyCreated(row.Id, row.ProjectId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        static PurchaseOrder Map(this CreatePurchaseOrderRequest request, PurchaseOrder row)
        {
            row.SupplierId = request.Supplier!.Id;
            row.QuoteCurrency = request.QuoteCurrency.Id;
            row.PurchaseOrderCurrency = request.PurchaseOrderCurrency.Id;
            row.PurchaseorderName = request.Name;
            row.PurchaseOrderStatus = PurchaseOrderStatusEnum.Created.Id;
            row.PurchaseRequisition = request.PurchaseRequisition;
            row.QuoteNo = request.QuoteNo;
            row.CurrencyDate = request.CurrencyDate!.Value;
            row.ProjectAccount = request.ProjectAccount;
            row.IsAlteration = request.IsAlteration;
            row.IsCapitalizedSalary = request.IsCapitalizedSalary;
            row.IsProductiveAsset = request.IsProductiveAsset;
            row.SPL = request.SPL;
            row.USDCOP = request.USDCOP;
            row.USDEUR = request.USDEUR;
            row.TaxCode = request.TaxCode;
            row.CostCenter = request.CostCenter.Id;
            row.MainBudgetItemId=request.MainBudgetItemId;
            return row;
        }

    }

}
