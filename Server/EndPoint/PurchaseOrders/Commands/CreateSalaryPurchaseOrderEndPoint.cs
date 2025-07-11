using Server.Database.Entities.PurchaseOrders;
using Shared.Enums.CurrencyEnums;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.PurchaseOrders.Requests;

namespace Server.EndPoint.PurchaseOrders.Commands
{
    public static class CreateSalaryPurchaseOrderEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PurchaseOrders.EndPoint.CreateSalary, async (CreateSalaryPurchaseOrderRequest Data, IRepository Repository) =>
                {

                    var row = PurchaseOrder.Create(Data.ProjectId);


                    await Repository.AddAsync(row);
                    Data.MapSalary(row);
                    foreach (var item in Data.SelectedPurchaseOrderItems)
                    {
                        var rowitem = PurchaseOrderItem.Create(row.Id, item.BudgetItemId);
                        rowitem.Name = item.Name;
                        rowitem.UnitaryValueQuoteCurrency = Data.ValueUSD;
                        rowitem.Quantity = 1;
                        rowitem.Order = 1;
                        var Received = rowitem.AddPurchaseOrderReceived();
                        Received.CurrencyDate = Data.SalaryDate;
                        Received.USDEUR = Data.ReceivingUSDEUR;
                        Received.USDCOP = Data.ReceivingUSDCOP;
                        Received.ValueReceivedCurrency = Data.ValueUSD;
                        Received.Order = 1;
                        await Repository.AddAsync(Received);
                        await Repository.AddAsync(rowitem);
                    }


                    List<string> cache = [.. StaticClass.PurchaseOrders.Cache.KeyClosed(row.Id, row.ProjectId, row.MainBudgetItemId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        static PurchaseOrder MapSalary(this CreateSalaryPurchaseOrderRequest request, PurchaseOrder row)
        {
           
            row.QuoteCurrency = CurrencyEnum.USD.Id;
            row.PurchaseOrderCurrency = CurrencyEnum.USD.Id;
            row.PurchaseorderName = request.Name;
            row.PurchaseOrderStatus = PurchaseOrderStatusEnum.Closed.Id;
            row.IsNotEditable = true;
            row.CurrencyDate = request.SalaryDate!.Value;

            row.ProjectAccount = request.ProjectAccount;
            row.IsAlteration = false;
            row.IsCapitalizedSalary = true;
            row.IsProductiveAsset = request.IsProductiveAsset;
            row.SPL = request.SPL;
            row.USDCOP = request.USDCOP;
            row.USDEUR = request.USDEUR;
            row.TaxCode = request.TaxCode;
            row.CostCenter = request.CostCenter.Id;
            row.MainBudgetItemId = request.MainBudgetItemId;
            return row;
        }

    }

}
