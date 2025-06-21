using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.OtherTasks.Records
{
   public class GetOtherTaskByIdRequest : GetByIdMessageResponse, IGetById
    {
   
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.OtherTasks.EndPoint.GetById;
        public override string ClassName => StaticClass.OtherTasks.ClassName;
    }

}
