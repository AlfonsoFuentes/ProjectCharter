using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.PurchaseOrders.Requests
{
    public class DeletePurchaseOrderRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.PurchaseOrders.ClassName;
     
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.PurchaseOrders.EndPoint.Delete;
    }
}
