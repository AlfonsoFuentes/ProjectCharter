using Server.Database.Entities.PurchaseOrders;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.PurchaseOrders.Requests;

namespace Server.EndPoint.PurchaseOrders.Commands
{
    public static class EditCreatedPurchaseOrderEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PurchaseOrders.EndPoint.EditCreated, async (EditPurchaseOrderCreatedRequest Data, IRepository Repository) =>
                {
                    Func<IQueryable<PurchaseOrder>, IIncludableQueryable<PurchaseOrder, object>> Includes = x => x
                     .Include(x => x.PurchaseOrderItems)
                    ;

                    Expression<Func<PurchaseOrder, bool>> Criteria = x => x.Id == Data.Id;

                    var row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);
                    if (row == null) { return Result.Fail(Data.Fail); }

                    await Repository.UpdateAsync(row);
                    Data.Map(row);
                    foreach (var item in row.PurchaseOrderItems)
                    {
                        if (!Data.SelectedPurchaseOrderItems.Any(x => x.Id == item.Id))
                        {
                            await Repository.RemoveAsync(item);
                        }
                    }
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
                            rowitem.BudgetItemId=item.BudgetItemId;
                            rowitem.Order=item.Order;
                            //if (item.BasicResponse != null)
                            //{
                            //    rowitem.BasicEngineeringItemId = item.BasicResponse.Id;

                            //}
                        }


                    }

                    List<string> cache = [.. StaticClass.PurchaseOrders.Cache.KeyCreated(row.Id, row.ProjectId, row.MainBudgetItemId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        static PurchaseOrder Map(this EditPurchaseOrderCreatedRequest request, PurchaseOrder row)
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
            row.SPL = request.SPL;
            row.USDCOP = request.USDCOP;
            row.USDEUR = request.USDEUR;
            row.TaxCode = request.TaxCode;

            return row;
        }

    }
}
