using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.PurchaseOrders.Validators
{
    public class ValidatePurchaseOrderRequisitionRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string PurchaseRequisition { get; set; } = string.Empty;

        public string EndPointName => StaticClass.PurchaseOrders.EndPoint.ValidatePR;

        public override string Legend => PurchaseRequisition;

        public override string ClassName => StaticClass.PurchaseOrders.ClassName;
    }

}
