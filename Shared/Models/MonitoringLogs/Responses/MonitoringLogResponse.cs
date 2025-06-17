using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MonitoringLogs.Responses
{
    public class MonitoringLogResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.MonitoringLogs.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.MonitoringLogs.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public DateTime? InitialDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
        public string sInitialDate => InitialDate?.ToString("d") ?? string.Empty;
        public string sEndDate => EndDate?.ToString("d") ?? string.Empty;
        public Guid ProjectId { get; set; }
        public string ClosingText { get; set; } = string.Empty;
    }
}
