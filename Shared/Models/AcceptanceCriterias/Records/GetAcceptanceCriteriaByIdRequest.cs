using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.AcceptanceCriterias.Records
{
   public class GetAcceptanceCriteriaByIdRequest : GetByIdMessageResponse, IGetById
    {
       
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.AcceptanceCriterias.EndPoint.GetById;
        public override string ClassName => StaticClass.AcceptanceCriterias.ClassName;
    }

}
