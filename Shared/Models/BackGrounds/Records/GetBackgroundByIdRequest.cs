using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Backgrounds.Records
{
   public class GetBackgroundByIdRequest : GetByIdMessageResponse, IGetById
    {
        
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.BackGrounds.EndPoint.GetById;
        public override string ClassName => StaticClass.BackGrounds.ClassName;
    }

}
