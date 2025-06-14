using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.DeliverableGanttTasks.Validators
{
    public class ValidateDeliverableGanttTaskRequest : ValidateMessageResponse, IRequest
    {
        public bool IsDeliverable { get; set; } 
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public Guid DeliverableId { get; set; }
        public string EndPointName => StaticClass.DeliverableGanttTasks.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.DeliverableGanttTasks.ClassName;
    }
}
