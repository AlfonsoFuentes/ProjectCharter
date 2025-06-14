using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Resources.Records
{
   public class GetResourceByIdRequest : GetByIdMessageResponse, IGetById
    {
   
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Resources.EndPoint.GetById;
        public override string ClassName => StaticClass.Resources.ClassName;
    }

}
