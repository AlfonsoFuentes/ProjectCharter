using Shared.Models.FileResults.Generics.Request;
using Shared.Models.PurchaseOrders.Responses;
using System.Text.Json.Serialization;

namespace Shared.Models.PurchaseOrders.Requests
{
    public class CreateSalaryPurchaseOrderRequest : PurchaseOrderResponse, IRequest, ICreateMessageResponse
    {
        public string EndPointName => StaticClass.PurchaseOrders.EndPoint.CreateSalary;

        public string Legend => Name;
        public string ClassName => StaticClass.PurchaseOrders.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessageCreated(Legend, ClassName);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessageCreated(Legend, ClassName);
        public double ValueUSD { get; set; }

        public DateTime? SalaryDate { get; set; } = DateTime.UtcNow;

    }
}
