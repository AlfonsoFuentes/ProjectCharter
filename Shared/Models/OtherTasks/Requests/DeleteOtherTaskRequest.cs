using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.OtherTasks.Requests
{
    public class DeleteOtherTaskRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.OtherTasks.ClassName;
      
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.OtherTasks.EndPoint.Delete;
    }
}
