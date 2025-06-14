using Shared.Models.FileResults.Generics.Request;
using Shared.Models.PurchaseOrders.Responses;

namespace Shared.Models.PurchaseOrders.Requests
{
    public class EditPurchaseOrderCreatedRequest : PurchaseOrderResponse, IRequest, ICreateMessageResponse
    {
      
        public string EndPointName => StaticClass.PurchaseOrders.EndPoint.EditCreated;
        public string Legend => Name;
        public string ClassName => StaticClass.PurchaseOrders.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessageCreated(Legend, ClassName);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessageCreated(Legend, ClassName);
    }
   
}
