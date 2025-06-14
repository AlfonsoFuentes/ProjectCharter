using Shared.Enums.CurrencyEnums;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Suppliers.Responses
{
    public class SupplierResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.Suppliers.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.Suppliers.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public string VendorCode { get; set; } = string.Empty;

        public string NickName { get; set; } = string.Empty;
        public string TaxCodeLP { get; set; } = "721031";
        public string TaxCodeLD { get; set; } = "751545";
        public string NickLargeName => $"{NickName} - {Name}";
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? ContactName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? ContactEmail { get; set; } = string.Empty;
        public CurrencyEnum SupplierCurrency { get; set; } = CurrencyEnum.COP;
      
    }
}
