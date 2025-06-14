using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Bennefits.Records
{
   public class GetBennefitByIdRequest : GetByIdMessageResponse, IGetById
    {
       
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Bennefits.EndPoint.GetById;
        public override string ClassName => StaticClass.Bennefits.ClassName;
    }

}
