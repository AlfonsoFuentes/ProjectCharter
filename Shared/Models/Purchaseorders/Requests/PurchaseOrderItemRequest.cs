using Shared.Enums.CurrencyEnums;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.BudgetItems.Responses;

namespace Shared.Models.PurchaseOrders.Requests
{
    public class PurchaseOrderItemRequest
    {
        public Guid Id { get; set; } = Guid.Empty;
        BudgetItemWithPurchaseOrdersResponse? _BudgetItem;
        public BudgetItemWithPurchaseOrdersResponse? BudgetItem
        {
            get { return _BudgetItem; }
            set
            {
                _BudgetItem = value;
                BudgetItemId = value == null ? Guid.Empty : value.Id;
            }
        }
        public int Order { get; set; }
        public Guid BudgetItemId { get; set; } = Guid.Empty;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        CurrencyEnum _QuoteCurrency = CurrencyEnum.None;
        public CurrencyEnum QuoteCurrency
        {
            get => _QuoteCurrency;
            set => SetQuoteCurrency(value);
        }
        public string Name { set; get; } = string.Empty;
        public string NomenclatoreName => BudgetItem == null ? string.Empty : BudgetItem.NomenclatoreName;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public double Quantity { get; set; }
        public double UnitaryQuoteCurrency { get; set; }
        public double TotalQuoteCurrency => UnitaryQuoteCurrency * Quantity;
        public double UnitaryPurchaseOrderCurrency =>
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? UnitaryUSD :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? UnitaryUSD * USDCOP :
             UnitaryUSD * USDEUR;
        public double TotalPurchaseOrderCurrency => UnitaryPurchaseOrderCurrency * Quantity;

        public double ActualCurrency => PurchaseOrderItemReceiveds.Sum(x => x.ValueReceivedCurrency);
        public double ActualUSD => PurchaseOrderItemReceiveds.Sum(x => x.ValueReceivedUSD);



        public double CommitmentCurrency => TotalPurchaseOrderCurrency - ActualCurrency;
        public double CommitmentUSD => TotalUSD - ActualUSD;

        public double UnitaryUSD => QuoteCurrency.Id == CurrencyEnum.USD.Id ? UnitaryQuoteCurrency :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? USDCOP == 0 ? 0 : UnitaryQuoteCurrency / USDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? USDEUR == 0 ? 0 : UnitaryQuoteCurrency / USDEUR :
            0;

        public double TotalUSD => UnitaryUSD * Quantity;

        public double BudgetUSD => BudgetItem == null ? 0 : BudgetItem.BudgetUSD;
        public double AssignedUSD => BudgetItem == null ? 0 : BudgetItem.AssignedUSD + TotalUSD;
        public double ToCommitUSD => BudgetUSD - AssignedUSD;


        public void SetQuoteCurrency(CurrencyEnum _quoteCurrency)
        {
            var oldUnitaryValueUSD = UnitaryUSD;
            _QuoteCurrency = _quoteCurrency;
            UnitaryQuoteCurrency =
                _QuoteCurrency.Id == CurrencyEnum.USD.Id ? oldUnitaryValueUSD :
                _QuoteCurrency.Id == CurrencyEnum.COP.Id ? oldUnitaryValueUSD * USDCOP :
                oldUnitaryValueUSD * USDEUR;



        }
        public double ReceivingUSDCOP { get; set; } = 0;
        public double ReceivingUSDEUR { get; set; } = 0;
        public double ReceivingValueCurrency { get; set; }

        public double ReceivingValueUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ReceivingValueCurrency :
           PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? ReceivingUSDCOP == 0 ? 0 : ReceivingValueCurrency / ReceivingUSDCOP :
           PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? ReceivingUSDEUR == 0 ? 0 : ReceivingValueCurrency / ReceivingUSDEUR :
           0;
        public double NewActualCurrency => ActualCurrency + ReceivingValueCurrency;
        public double NewActualUSD => ActualUSD + ReceivingValueUSD;
        public double NewCommitmentCurrency => TotalPurchaseOrderCurrency - NewActualCurrency;
        public double NewCommitmentUSD => TotalUSD - NewActualUSD;
        public double PendingToReceiveCurrency => TotalPurchaseOrderCurrency - ActualCurrency - ReceivingValueCurrency;
        public double PendingToReceiveUSD => TotalUSD - ActualUSD - ReceivingValueUSD;
       
        public List<PurchaseOrderItemReceivedRequest> PurchaseOrderItemReceiveds { get; set; } = new();
    }
}
