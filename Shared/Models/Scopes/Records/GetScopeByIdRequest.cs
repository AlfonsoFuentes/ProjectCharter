using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Scopes.Records
{
   public class GetScopeByIdRequest : GetByIdMessageResponse, IGetById
    {
     
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Scopes.EndPoint.GetById;
        public override string ClassName => StaticClass.Scopes.ClassName;
    }

}
