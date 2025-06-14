using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Acquisitions.Records
{
   public class GetAcquisitionByIdRequest : GetByIdMessageResponse, IGetById
    {
   
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Acquisitions.EndPoint.GetById;
        public override string ClassName => StaticClass.Acquisitions.ClassName;
    }

}
