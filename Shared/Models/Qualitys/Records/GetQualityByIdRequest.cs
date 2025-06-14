using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Qualitys.Records
{
   public class GetQualityByIdRequest : GetByIdMessageResponse, IGetById
    {
   
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Qualitys.EndPoint.GetById;
        public override string ClassName => StaticClass.Qualitys.ClassName;
    }

}
