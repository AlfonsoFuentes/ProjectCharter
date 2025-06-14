using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.ExpertJudgements.Records
{
   public class GetExpertJudgementByIdRequest : GetByIdMessageResponse, IGetById
    {
       
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.ExpertJudgements.EndPoint.GetById;
        public override string ClassName => StaticClass.ExpertJudgements.ClassName;
    }

}
