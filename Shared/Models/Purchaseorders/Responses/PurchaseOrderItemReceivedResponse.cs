using Shared.Enums.CurrencyEnums;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class PurchaseOrderItemReceivedResponse : BaseResponse
    {
        public string ItemName { get; set; } = string.Empty;
        public string NomenclatoreName { get; set; } = string.Empty;
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

    
    }

}
