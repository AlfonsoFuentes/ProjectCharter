using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.KnownRisks.Records
{
   public class GetKnownRiskByIdRequest : GetByIdMessageResponse, IGetById
    {
   
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.KnownRisks.EndPoint.GetById;
        public override string ClassName => StaticClass.KnownRisks.ClassName;
    }

}
