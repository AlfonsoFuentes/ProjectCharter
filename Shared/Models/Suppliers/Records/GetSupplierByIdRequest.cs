using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Suppliers.Records
{
   public class GetSupplierByIdRequest : GetByIdMessageResponse, IGetById
    {
       
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Suppliers.EndPoint.GetById;
        public override string ClassName => StaticClass.Suppliers.ClassName;
    }

}
