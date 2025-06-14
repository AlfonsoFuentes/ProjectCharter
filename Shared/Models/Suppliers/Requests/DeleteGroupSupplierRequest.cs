using Shared.Models.Suppliers.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Suppliers.Requests
{
    public class DeleteGroupSupplierRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of Supplier";

        public override string ClassName => StaticClass.Suppliers.ClassName;

        public HashSet<SupplierResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Suppliers.EndPoint.DeleteGroup;
    }
}
