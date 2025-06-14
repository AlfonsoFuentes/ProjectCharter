using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.PurchaseOrders.Validators
{
    public class ValidatePurchaseOrderNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId {  get; set; }
        public string EndPointName => StaticClass.PurchaseOrders.EndPoint.ValidateName;

        public override string Legend => Name;

        public override string ClassName => StaticClass.PurchaseOrders.ClassName;
    }

}
