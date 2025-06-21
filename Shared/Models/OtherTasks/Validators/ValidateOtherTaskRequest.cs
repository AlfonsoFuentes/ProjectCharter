using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.OtherTasks.Validators
{
  
    public class ValidateOtherTaskRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.OtherTasks.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.OtherTasks.ClassName;
    }
}
