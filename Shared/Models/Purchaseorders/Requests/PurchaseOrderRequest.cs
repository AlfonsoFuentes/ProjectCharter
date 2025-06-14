using Shared.Enums.CostCenter;
using Shared.Enums.CurrencyEnums;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Suppliers.Responses;

namespace Shared.Models.PurchaseOrders.Requests
{
    public abstract class PurchaseOrderRequest : CreateMessageResponse
    {
        public Guid Id { get; set; }
        public Guid MainBudgetItemId { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public DateTime? ExpectedDate { get; set; } = DateTime.UtcNow;
        public SupplierResponse? Supplier { get; set; }
        public Guid? SupplierId => Supplier == null ? Guid.Empty : Supplier.Id;
        public string VendorCode => Supplier == null ? string.Empty : Supplier.VendorCode;
        public string SPL => IsAlteration ? "0735015000" : "151605000";
        public string TaxCodeLD { get; set; } = "751545";
        public string TaxCodeLP { get; set; } = "721031";
        public string QuoteNo { get; set; } = string.Empty;
   
        public bool IsAlteration { get; set; }
        public bool IsProductive { get; set; }
        public bool IsCapitalizedSalary { get; set; }
        public CostCenterEnum CostCenter { get; set; } = CostCenterEnum.None;
        public string TaxCode => IsAlteration || !IsProductive ? TaxCodeLP : TaxCodeLD;
        public DateTime? CurrencyDate { get; set; } = DateTime.UtcNow;
    
        public string PurchaseRequisition { get; set; } = string.Empty;
        public Guid ProjectId { get; set; } = Guid.Empty;
        public string ProjectAccount { get; set; } = string.Empty;
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
        public string Name { set; get; } = string.Empty;

   

        public virtual List<PurchaseOrderItemReceivedRequest> PurchaseOrderItemReceiveds=> PurchaseOrderItems.SelectMany(x => x.PurchaseOrderItemReceiveds).ToList();
        public virtual List<PurchaseOrderItemRequest> PurchaseOrderItems { set; get; } = new();
        public void RemoveItem(PurchaseOrderItemRequest item)
        {
            PurchaseOrderItems.Remove(item);
            int order = 1;
            PurchaseOrderItems.ForEach(item =>
            {

                item.Order = order;
                order++;
            });
        }
        public virtual List<PurchaseOrderItemRequest> SelectedPurchaseOrderItems => PurchaseOrderItems.Where(x => x.BudgetItemId != Guid.Empty).OrderBy(x => x.Order).ToList();
        int LastOrder => SelectedPurchaseOrderItems.Count == 0 ? 0 : SelectedPurchaseOrderItems.MaxBy(x => x.Order)!.Order;
        public void AddItem(PurchaseOrderItemRequest item)
        {
            PurchaseOrderItems.Add(item);
            item.PurchaseOrderCurrency = PurchaseOrderCurrency;
            item.USDCOP = USDCOP;
            item.USDEUR = USDEUR;
            item.QuoteCurrency = QuoteCurrency;
            item.Order = LastOrder + 1;
        }
        public bool IsAnyValueNotDefined => SelectedPurchaseOrderItems.Any(x => x.TotalQuoteCurrency <= 0);
        public bool IsAnyNameEmpty => SelectedPurchaseOrderItems.Any(x => string.IsNullOrEmpty(x.Name));
        public double TotalQuoteCurrency => PurchaseOrderItems.Count == 0 ? 0 : PurchaseOrderItems.Sum(x => x.TotalQuoteCurrency);
        public double TotalPurchaseOrderCurrency => PurchaseOrderItems.Count == 0 ? 0 : PurchaseOrderItems.Sum(x => x.TotalPurchaseOrderCurrency);
        public double TotalUSD => PurchaseOrderItems.Count == 0 ? 0 : PurchaseOrderItems.Sum(x => x.TotalUSD);
      

        public bool IsAnyPendingToReceiveLessThanZero => PurchaseOrderItems.Any(x => x.PendingToReceiveCurrency < 0);   
        public double PendingToReceiveCurrency => PurchaseOrderItems.Count == 0 ? 0 : PurchaseOrderItems.Sum(x => x.PendingToReceiveCurrency);
        public DateTime? ReceivingDate { get; set; } = DateTime.UtcNow;
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

        public bool IsCompletedReceived => PurchaseOrderItems.Sum(x => x.PendingToReceiveCurrency) == 0;
        public double TotalReceivingUSD => PurchaseOrderItems.Count == 0 ? 0 : PurchaseOrderItems.Sum(x => x.ReceivingValueUSD);      
        public double ActualUSD => PurchaseOrderItems.Count == 0 ? 0 : PurchaseOrderItems.Sum(x => x.ActualUSD);
        public double ActualCurrency => PurchaseOrderItems.Count == 0 ? 0 : PurchaseOrderItems.Sum(x => x.ActualCurrency);
      

    }
}
