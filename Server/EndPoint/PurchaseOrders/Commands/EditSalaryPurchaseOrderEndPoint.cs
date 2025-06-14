using Server.Database.Entities.PurchaseOrders;
using Shared.Enums.CurrencyEnums;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.PurchaseOrders.Requests;

namespace Server.EndPoint.PurchaseOrders.Commands
{
    public static class EditSalaryPurchaseOrderEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PurchaseOrders.EndPoint.EditSalary, async (EditPurchaseOrderSalaryRequest Data, IRepository Repository) =>
                {

                    Func<IQueryable<PurchaseOrder>, IIncludableQueryable<PurchaseOrder, object>> Includes = x => x
                     .Include(x => x.PurchaseOrderItems)
                    ;

                    Expression<Func<PurchaseOrder, bool>> Criteria = x => x.Id == Data.Id;

                    var row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);
                    if (row == null) { return Result.Fail(Data.Fail); }

                    Data.MapSalary(row);
                   


                    foreach (var item in Data.PurchaseOrderItemReceiveds)
                    {
                        var received = await Repository.GetByIdAsync<PurchaseOrderItemReceived>(item.Id);
                        if (received != null)
                        {

                            received.CurrencyDate = Data.SalaryDate;
                            received.USDEUR = item.USDEUR;
                            received.USDCOP = item.USDCOP;
                            received.ValueReceivedCurrency = item.ValueReceivedCurrency;
                            await Repository.UpdateAsync(received);
                        }

                    }

                    await Repository.UpdateAsync(row);
                    List<string> cache = [.. StaticClass.PurchaseOrders.Cache.KeyClosed(row.Id, row.ProjectId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        static PurchaseOrder MapSalary(this EditPurchaseOrderSalaryRequest request, PurchaseOrder row)
        {

            row.QuoteCurrency = CurrencyEnum.USD.Id;
            row.PurchaseOrderCurrency = CurrencyEnum.USD.Id;
            row.PurchaseorderName = request.Name;
            row.PurchaseOrderStatus = PurchaseOrderStatusEnum.Closed.Id;
            row.IsNotEditable = true;
            row.CurrencyDate = request.SalaryDate!.Value;
            row.ProjectAccount = request.ProjectAccount;
            row.IsAlteration = request.IsAlteration;
            row.IsCapitalizedSalary = request.IsCapitalizedSalary;
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
