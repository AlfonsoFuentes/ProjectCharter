using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Objectives.Records
{
   public class GetObjectiveByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Objectives.EndPoint.GetById;
        public override string ClassName => StaticClass.Objectives.ClassName;
    }

}
