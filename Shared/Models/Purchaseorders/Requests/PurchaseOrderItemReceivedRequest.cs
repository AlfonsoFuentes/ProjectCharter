using Shared.Enums.CurrencyEnums;

namespace Shared.Models.PurchaseOrders.Requests
{
    public class PurchaseOrderItemReceivedRequest
    {
        public string ItemName { get; set; } = string.Empty;
        public string NomenclatoreName { get; set; } = string.Empty;
        public int Order { get; set; } = 0;
        public Guid Id { get; set; } = Guid.Empty;
        public double ValueReceivedCurrency { get; set; }
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }

        public DateTime? CurrencyDate { get; set; }
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public double ValueReceivedUSD =>
          PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ValueReceivedCurrency :
          PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? ValueReceivedCurrency / USDCOP :
          PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? ValueReceivedCurrency / USDEUR :
            0;
        public string sValueReceivedUSD => string.Format(new CultureInfo("en-US"), "{0:C0}", Math.Round(ValueReceivedUSD, 2));
    }
}
