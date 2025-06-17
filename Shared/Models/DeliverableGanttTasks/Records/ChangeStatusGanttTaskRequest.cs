using Shared.Enums.TaskStatus;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.DeliverableGanttTasks.Records
{
    public class ChangeStatusGanttTaskRequest : IMessageResponse, IRequest
    {
        public string Legend => "Update Status";

        public string ClassName => StaticClass.DeliverableGanttTasks.ClassName;
        public string ActionType => "Update Status";
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        public string EndPointName => StaticClass.DeliverableGanttTasks.EndPoint.UpdateStatus;

        public Guid Id { get; set; } = Guid.Empty;
        public GanttTaskStatusEnum Status { get; set; } = GanttTaskStatusEnum.NotInitiated;
        public Guid ProjectId { get; set; } = Guid.Empty;
    }
}
