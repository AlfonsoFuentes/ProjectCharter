using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Requirements.Records
{
   public class GetRequirementByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Requirements.EndPoint.GetById;
        public override string ClassName => StaticClass.Requirements.ClassName;
    }

}
