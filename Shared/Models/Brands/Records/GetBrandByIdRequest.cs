using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Brands.Records
{
   public class GetBrandByIdRequest : GetByIdMessageResponse, IGetById
    {
       
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Brands.EndPoint.GetById;
        public override string ClassName => StaticClass.Brands.ClassName;
    }

}
