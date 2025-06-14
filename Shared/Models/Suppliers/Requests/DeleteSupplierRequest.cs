using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Suppliers.Requests
{
    public class DeleteSupplierRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.Suppliers.ClassName;
     
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Suppliers.EndPoint.Delete;
    }
}
