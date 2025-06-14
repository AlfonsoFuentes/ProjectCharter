using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.PurchaseOrders.Records
{
   public class GetPurchaseOrderByIdRequest : GetByIdMessageResponse, IGetById
    {
       
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.PurchaseOrders.EndPoint.GetById;
        public override string ClassName => StaticClass.PurchaseOrders.ClassName;
    }

}
