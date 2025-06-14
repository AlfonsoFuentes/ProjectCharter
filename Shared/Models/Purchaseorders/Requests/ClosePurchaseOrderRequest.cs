using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.PurchaseOrders.Requests
{
    public class ClosePurchaseOrderRequest : UpdateMessageResponse, IRequest
    {


        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.PurchaseOrders.EndPoint.Close;

        public override string Legend => Name;

        public override string ClassName => StaticClass.PurchaseOrders.ClassName;
    }
}
