using Shared.Enums.CurrencyEnums;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.BudgetItems.BasicEngineeringItems;
using Shared.Models.BudgetItems.Responses;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class PurchaseOrderItemResponse : BaseResponse
    {
        public Guid BudgetItemId { get; set; } = Guid.Empty;

        BudgetItemWithPurchaseOrdersResponse? _BudgetItem;
        public BudgetItemWithPurchaseOrdersResponse? BudgetItem
        {
            get { return _BudgetItem; }
            set
            {
                _BudgetItem = value;
                if (_BudgetItem != null)
                {
                    BudgetItemId = _BudgetItem.Id;

                }
            }
        }
        //public Guid BasicReponseId { get; set; } = Guid.Empty;

        //BasicResponse? _BasicReponse;
        //public BasicResponse? BasicResponse
        //{
        //    get { return _BasicReponse; }
        //    set
        //    {
        //        _BasicReponse = value;
        //        if (_BasicReponse != null)
        //        {
        //            BasicReponseId = _BasicReponse.Id;

        //        }
        //    }
        ////}
        //public string BasicResponseName => BasicResponse == null ? string.Empty : BasicResponse.Name;
        public string NomenclatoreName => BudgetItem == null ? string.Empty : BudgetItem.NomenclatoreName;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;
        public double UnitaryQuoteCurrency { get; set; }
        public double Quantity { get; set; }
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public List<PurchaseOrderItemReceivedResponse> PurchaseOrderItemReceiveds { get; set; } = new();
        public double TotalItemQuoteCurrency => UnitaryQuoteCurrency * Quantity;
        public double UnitaryValueUSD => QuoteCurrency.Id == CurrencyEnum.USD.Id ? UnitaryQuoteCurrency :
        QuoteCurrency.Id == CurrencyEnum.COP.Id ? USDCOP == 0 ? 0 : UnitaryQuoteCurrency / USDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? USDEUR == 0 ? 0 : UnitaryQuoteCurrency / USDEUR : 0;
        public double TotalPurchaseOrderUSD => UnitaryValueUSD * Quantity;
        public double UnitaryValuePurchaseOrderCurrency => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? UnitaryValueUSD :
        PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? USDCOP == 0 ? 0 : UnitaryValueUSD * USDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? USDEUR == 0 ? 0 : UnitaryValueUSD * USDEUR : 0;
        public double TotalPurchaseOrderCurrency => UnitaryValuePurchaseOrderCurrency * Quantity;
        public double BudgetUSD => BudgetItem == null ? 0 : BudgetItem.BudgetUSD;
        public double ActualBudgetItemUSD => BudgetItem == null ? 0 : BudgetItem.ActualUSD;
        public double PotentialBudgetItemUSD => BudgetItem == null ? 0 : BudgetItem.PotentialUSD;
        public double CommitmentBudgetItemUSD => BudgetItem == null ? 0 : BudgetItem.CommitmentUSD;
        public double AssignedBudgetItemUSD => ActualBudgetItemUSD + CommitmentBudgetItemUSD + PotentialBudgetItemUSD + TotalPurchaseOrderUSD;
        public double ToCommitItemUSD => BudgetUSD - AssignedBudgetItemUSD;
        public double ReceivingUSDCOP { get; set; } = 0;
        public double ReceivingUSDEUR { get; set; } = 0;
        public double ReceivingValueCurrency { get; set; }

        public double ReceivingValueUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ReceivingValueCurrency :
           PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? ReceivingUSDCOP == 0 ? 0 : ReceivingValueCurrency / ReceivingUSDCOP :
           PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? ReceivingUSDEUR == 0 ? 0 : ReceivingValueCurrency / ReceivingUSDEUR :
           0;
        public double NewActualPurchaseOrderUSD => ActualPurchaseOrderUSD + ReceivingValueUSD;
        public double NewCommitmentPurchaseOrderUSD => TotalPurchaseOrderUSD - NewActualPurchaseOrderUSD;
        public double PendingToReceiveUSD => TotalPurchaseOrderUSD - NewActualPurchaseOrderUSD;
        public double PendingToReceiveCurrency => TotalPurchaseOrderCurrency - ReceivingValueCurrency - ActualPurchaseOrderCurrency;
        public double ActualPurchaseOrderCurrency => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? 0 : PurchaseOrderItemReceiveds.Sum(x => x.ValueReceivedCurrency);
        public double ActualPurchaseOrderUSD => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? 0 : PurchaseOrderItemReceiveds.Sum(x => x.ValueReceivedUSD);
        public double CommitmentItemPurchaseOrderCurrency => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? 0 : TotalPurchaseOrderCurrency - ActualPurchaseOrderCurrency;
        public double CommitmentItemPurchaseOrderUSD =>
            PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id || PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Closed.Id ? 0 :
            TotalPurchaseOrderUSD - ActualPurchaseOrderUSD;
        public double PotentialItemPurchaseOrderUSD => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? TotalPurchaseOrderUSD : 0;
    }

}
