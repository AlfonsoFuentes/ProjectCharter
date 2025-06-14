using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Communications.Records
{
   public class GetCommunicationByIdRequest : GetByIdMessageResponse, IGetById
    {
   
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Communications.EndPoint.GetById;
        public override string ClassName => StaticClass.Communications.ClassName;
    }

}
