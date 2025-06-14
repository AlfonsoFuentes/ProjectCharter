using Server.Database.Entities.PurchaseOrders;
using Server.EndPoint.Suppliers.Queries;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.PurchaseOrders.Records;
using Shared.Models.PurchaseOrders.Responses;

namespace Server.EndPoint.PurchaseOrders.Queries
{
    public static class GetPurchaseOrderByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PurchaseOrders.EndPoint.GetById, async (GetPurchaseOrderByIdRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<PurchaseOrder, bool>> Criteria = x => x.Id == Data.Id;


                    Func<IQueryable<PurchaseOrder>, IIncludableQueryable<PurchaseOrder, object>> Includes = x => x
                    .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.BudgetItem!)
                    .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.BasicEngineeringItem!)
                    .Include(p => p.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                    .Include(x => x.Supplier!);
                    var cache = StaticClass.PurchaseOrders.Cache.GetById(Data.Id);
                    var row = await Repository.GetAsync(cache, Criteria: Criteria, Includes: Includes);
                    if (row == null)
                    {
                        return Result<PurchaseOrderResponse>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.PurchaseOrders.ClassLegend));
                    }
                    PurchaseOrderResponse response = row.Map();

                    return Result<PurchaseOrderResponse>.Success(response);

                });
            }
        }
        public static PurchaseOrderResponse Map(this PurchaseOrder row)
        {
            return new()
            {
                ProjectAccount = row.ProjectAccount,
                CurrencyDate = row.CurrencyDate,
                Id = row.Id,
                IsAlteration = row.IsAlteration,
                IsCapitalizedSalary = row.IsCapitalizedSalary,
                IsTaxEditable = row.IsTaxEditable,
                Name = row.PurchaseorderName,
                PurchaseOrderCurrency = row.PurchaseOrderCurrencyEnum,
                PurchaseOrderStatus = row.PurchaseOrderStatusEnum,
                CostCenter = row.CostCenterEnum,
                IsProductiveAsset = row.IsProductiveAsset,
                QuoteCurrency = row.QuoteCurrencyEnum,
                PurchaseRequisition = row.PurchaseRequisition,
                QuoteNo = row.QuoteNo,

                USDCOP = row.USDCOP,
                USDEUR = row.USDEUR,
                ApprovedDate = row.ApprovedDate,
                ClosedDate = row.ClosedDate,
                ExpectedDate = row.ExpectedDate,
                PONumber = row.PONumber,
                ProjectId = row.ProjectId,
                MainBudgetItemId = row.MainBudgetItemId,
                Supplier = row.Supplier == null ? null! : row.Supplier.Map(),

                PurchaseOrderItems = row.PurchaseOrderItems.Select(x => x.Map()).ToList(),

            };
        }

        public static PurchaseOrderItemResponse Map(this PurchaseOrderItem row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                BudgetItemId = row.BudgetItemId!.Value,
                Quantity = row.Quantity,
                PurchaseOrderCurrency = row.PurchaseOrderCurrency,
                QuoteCurrency = row.QuoteCurrency,
                UnitaryQuoteCurrency = row.UnitaryValueQuoteCurrency,
                PurchaseOrderStatus = row.PurchaseOrderStatus,
                USDCOP = row.USDCOP,
                USDEUR = row.USDEUR,
                Order = row.Order,
                PurchaseOrderItemReceiveds = row.PurchaseOrderReceiveds.Select(x => x.Map()).ToList(),
                BasicResponse = row.BasicEngineeringItem == null ? null! : row.BasicEngineeringItem.Map(),
               


            };
        }
        public static PurchaseOrderItemReceivedResponse Map(this PurchaseOrderItemReceived row)
        {
            return new()
            {
                ItemName = row.ItemName,
                NomenclatoreName = row.NomenclatoreName,
                Id = row.Id,
                USDEUR = row.USDEUR,
                USDCOP = row.USDCOP,
                CurrencyDate = row.CurrencyDate,
                ValueReceivedCurrency = row.ValueReceivedCurrency,
                PurchaseOrderCurrency = row.PurchaseOrderCurrency,
                Order = row.Order,
            };
        }

    }
}
