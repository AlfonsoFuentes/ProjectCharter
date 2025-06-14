using Shared.Enums.CostCenter;
using Shared.Enums.CurrencyEnums;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.Suppliers.Responses;
using System.Text.Json.Serialization;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class PurchaseOrderResponse : BaseResponse
    {
      
        public string SupplierName => Supplier == null ? string.Empty : Supplier.Name;
        public string SupplierNickName => Supplier == null ? string.Empty : Supplier.NickName;
        public SupplierResponse? Supplier { get; set; } = null!;
        public Guid? SupplierId => Supplier == null ? Guid.Empty : Supplier.Id;
        public string VendorCode => Supplier == null ? string.Empty : Supplier.VendorCode;
        public string SPL => IsAlteration ? "0735015000" : "151605000";
       
        public CostCenterEnum CostCenter { get; set; } = CostCenterEnum.None;
        public string TaxCodeLD { get; set; } = "751545";
        public string TaxCodeLP { get; set; } = "721031";
        public string TaxCode => IsAlteration || !IsProductiveAsset ? TaxCodeLP : TaxCodeLD;
        public string QuoteNo { get; set; } = string.Empty;
        CurrencyEnum _QuoteCurrency = CurrencyEnum.None;
        public CurrencyEnum QuoteCurrency
        {
            get { return _QuoteCurrency; }
            set
            {
                _QuoteCurrency = value;
                foreach (var item in PurchaseOrderItems)
                {
                    item.QuoteCurrency = _QuoteCurrency;
                }
            }
        }
        CurrencyEnum _PurchaseOrderCurrency = CurrencyEnum.None;
        public CurrencyEnum PurchaseOrderCurrency
        {
            get { return _PurchaseOrderCurrency; }
            set
            {
                _PurchaseOrderCurrency = value;
                foreach (var item in PurchaseOrderItems)
                {
                    item.PurchaseOrderCurrency = _PurchaseOrderCurrency;
                }
            }
        }
        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;
        public string PurchaseRequisition { get; set; } = string.Empty;
        public DateTime? ApprovedDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string sExpectedDate => ExpectedDate.HasValue ? ExpectedDate.Value.ToString("d") : string.Empty;
        public DateTime? ClosedDate { get; set; }
        public DateTime? ReceivingDate { get; set; } = DateTime.UtcNow;
        public string PONumber { get; set; } = string.Empty;
     
        double _USDCOP = 0;
        public double USDCOP
        {
            get { return _USDCOP; }
            set
            {
                _USDCOP = value;
                foreach (var item in PurchaseOrderItems)
                {
                    item.USDCOP = _USDCOP;
                }

            }
        }
        double _USDEUR = 0;
        public double USDEUR
        {
            get { return _USDEUR; }
            set
            {
                _USDEUR = value;
                foreach (var item in PurchaseOrderItems)
                {
                    item.USDEUR = _USDEUR;
                }

            }
        }
        public DateTime? CurrencyDate { get; set; }
        public string ProjectAccount { get; set; } = string.Empty;
        public bool IsAlteration { get; set; } = false;
        public bool IsCapitalizedSalary { get; set; } = false;
        public bool IsTaxEditable { get; set; } = false;
        public bool IsProductiveAsset { get; set; } = false;
        public Guid MainBudgetItemId { get; set; } = Guid.Empty;
        public Guid ProjectId { get; set; } = Guid.Empty;
        public List<PurchaseOrderItemResponse> PurchaseOrderItems { get; set; } = new();
        public virtual List<PurchaseOrderItemResponse> SelectedPurchaseOrderItems => PurchaseOrderItems.Where(x => x.BudgetItemId != Guid.Empty).OrderBy(x => x.Order).ToList();
        public List<PurchaseOrderItemReceivedResponse> PurchaseOrderItemReceiveds => PurchaseOrderItems.SelectMany(x => x.PurchaseOrderItemReceiveds).ToList();
        public double TotalPurchaseOrderCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
             PurchaseOrderItems.Sum(x => x.TotalPurchaseOrderCurrency);
        public double TotalQuoteCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.TotalItemQuoteCurrency);
        public double TotalUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
           PurchaseOrderItems.Sum(x => x.TotalPurchaseOrderUSD);
      
        public double ActualBudgetItemUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.ActualBudgetItemUSD);
        public double PotentialBudgetItemUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.PotentialBudgetItemUSD);
        public double CommitmentBudgetItemUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.CommitmentBudgetItemUSD);
       
        public double TotalReceivingValueUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.ReceivingValueUSD);
        public double TotalReceivingValueCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
           PurchaseOrderItems.Sum(x => x.ReceivingValueCurrency);
        public bool IsAnyValueNotDefined => SelectedPurchaseOrderItems.Any(x => x.TotalItemQuoteCurrency <= 0);
        public bool IsAnyNameEmpty => SelectedPurchaseOrderItems.Any(x => string.IsNullOrEmpty(x.Name));
        public void RemoveItem(PurchaseOrderItemResponse item)
        {
            PurchaseOrderItems.Remove(item);
            int order = 1;
            PurchaseOrderItems.ForEach(item =>
            {

                item.Order = order;
                order++;
            });
        }
        int LastOrder => SelectedPurchaseOrderItems.Count == 0 ? 0 : SelectedPurchaseOrderItems.MaxBy(x => x.Order)!.Order;
        public void AddItem(PurchaseOrderItemResponse item)
        {
            PurchaseOrderItems.Add(item);
            item.PurchaseOrderCurrency = PurchaseOrderCurrency;
            item.USDCOP = USDCOP;
            item.USDEUR = USDEUR;
            item.QuoteCurrency = QuoteCurrency;
            item.Order = LastOrder + 1;
        }
        double _ReceivingUSDCOP;
        double _ReceivingUSDEUR;
        public double ReceivingUSDCOP
        {
            get { return _ReceivingUSDCOP; }
            set
            {
                _ReceivingUSDCOP = value;
                foreach (var item in PurchaseOrderItems)
                {
                    item.ReceivingUSDCOP = _ReceivingUSDCOP;
                }
            }
        }
        public double ReceivingUSDEUR
        {
            get { return _ReceivingUSDEUR; }
            set
            {
                _ReceivingUSDEUR = value;
                foreach (var item in PurchaseOrderItems)
                {
                    item.ReceivingUSDEUR = _ReceivingUSDEUR;
                }
            }
        }
        public bool IsCompletedReceived => SelectedPurchaseOrderItems.Sum(x => x.PendingToReceiveCurrency) == 0;
        public bool IsAnyPendingToReceiveLessThanZero => SelectedPurchaseOrderItems.Any(x => x.PendingToReceiveCurrency < 0);
        public double ActualPurchaseOrderCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.ActualPurchaseOrderCurrency);
        public double CommitmentPurchaseOrderCurrency => TotalPurchaseOrderCurrency - ActualPurchaseOrderCurrency;

        public double ActualPurchaseOrderUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
           PurchaseOrderItems.Sum(x => x.ActualPurchaseOrderUSD);
        public double PotentialPurchaseOrderUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.PotentialItemPurchaseOrderUSD);
        public double CommitmentPurchaseOrderUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.CommitmentItemPurchaseOrderUSD);
        [JsonIgnore]
        public List<PurchaseOrderResponse> ProjectPurchaseOrders { get; set; } = new();

    }
}
