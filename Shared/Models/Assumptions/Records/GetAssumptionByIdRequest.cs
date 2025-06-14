using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Assumptions.Records
{
   public class GetAssumptionByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid ProjectId { get; set; }

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Assumptions.EndPoint.GetById;
        public override string ClassName => StaticClass.Assumptions.ClassName;
    }

}
