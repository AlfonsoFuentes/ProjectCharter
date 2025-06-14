using Shared.Models.FileResults.Generics.Request;
using Shared.Models.PurchaseOrders.Responses;

namespace Shared.Models.PurchaseOrders.Requests
{
    public class EditPurchaseOrderSalaryRequest : PurchaseOrderResponse, IRequest, ICreateMessageResponse
    {


        public string EndPointName => StaticClass.PurchaseOrders.EndPoint.EditSalary;
        public string Legend => Name;
        public string ClassName => StaticClass.PurchaseOrders.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessageCreated(Legend, ClassName);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessageCreated(Legend, ClassName);
        public DateTime? SalaryDate { get; set; } = DateTime.UtcNow;
        public double ValueUSD { get; set; }
    }
}
