using Server.Database.Entities.PurchaseOrders;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.PurchaseOrders.Requests;

namespace Server.EndPoint.PurchaseOrders.Commands
{
    public static class ReceivePurchaseOrderApprovedEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PurchaseOrders.EndPoint.Receive, async (ReceivePurchaseOrderApprovedRequest Data, IRepository Repository) =>
                {
                    Func<IQueryable<PurchaseOrder>, IIncludableQueryable<PurchaseOrder, object>> Includes = x => x
                     .Include(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                    ;

                    Expression<Func<PurchaseOrder, bool>> Criteria = x => x.Id == Data.Id;

                    var row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);
                    if (row == null) { return Result.Fail(Data.Fail); }
                    var currentReceived = row.ActualCurrency;
                    var POValueCurrency = row.TotalPurchaseOrderCurrency;
                    int order = 1;
                    foreach (var item in Data.SelectedPurchaseOrderItems)
                    {
                        PurchaseOrderItem rowitem = null!;
                        if (row.PurchaseOrderItems.Any(x => x.Id == item.Id))
                        {
                            rowitem = row.PurchaseOrderItems.First(x => x.Id == item.Id);
                            await Repository.UpdateAsync(rowitem);
                        }
                        else
                        {
                            rowitem = PurchaseOrderItem.Create(row.Id, item.BudgetItemId);
                            await Repository.AddAsync(rowitem);
                        }
                        if (rowitem != null)
                        {
                            rowitem.Name = item.Name;
                            rowitem.UnitaryValueQuoteCurrency = item.UnitaryQuoteCurrency;
                            rowitem.Quantity = item.Quantity;
                            rowitem.BudgetItemId = item.BudgetItemId;
                            rowitem.Order = item.Order;
                            if (item.BasicResponse != null)
                            {
                                rowitem.BasicEngineeringItemId = item.BasicResponse.Id;

                            }
                        }


                    }
                    foreach (var item in Data.PurchaseOrderItems)
                    {
                        var purchaseorderitem = await Repository.GetByIdAsync<PurchaseOrderItem>(item.Id);
                        if (purchaseorderitem != null)
                        {
                            var Received = purchaseorderitem.AddPurchaseOrderReceived();
                            Received.CurrencyDate = Data.ReceivingDate;
                            Received.USDEUR = Data.ReceivingUSDEUR;
                            Received.USDCOP = Data.ReceivingUSDCOP;
                            Received.ValueReceivedCurrency = item.ReceivingValueCurrency;
                            Received.Order = order;
                            await Repository.AddAsync(Received);
                            currentReceived += item.ReceivingValueCurrency;
                            order++;
                        }

                    }
                    if (Math.Abs(currentReceived - POValueCurrency) <= 1e-3)
                    {
                        row.PurchaseOrderStatus = PurchaseOrderStatusEnum.Closed.Id;
                        row.ClosedDate = Data.IsCompletedReceived ? DateTime.UtcNow : null;
                    }
                    else
                    {
                        row.PurchaseOrderStatus = PurchaseOrderStatusEnum.Receiving.Id;
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


       

    }
}
