using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Constrainsts.Records
{
   public class GetConstrainstByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Constrainsts.EndPoint.GetById;
        public override string ClassName => StaticClass.Constrainsts.ClassName;
    }

}
