using Shared.Models.FileResults.Generics.Request;
using Shared.Models.PurchaseOrders.Responses;

namespace Shared.Models.PurchaseOrders.Requests
{
    public class EditPurchaseOrderClosedRequest : PurchaseOrderResponse, IRequest, ICreateMessageResponse
    {


        public string EndPointName => StaticClass.PurchaseOrders.EndPoint.EditClosed;
        public string Legend => Name;
        public string ClassName => StaticClass.PurchaseOrders.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessageCreated(Legend, ClassName);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessageCreated(Legend, ClassName);
    }
}
