using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MonitoringLogs.Validators
{
 
    public class ValidateMonitoringLogRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.MonitoringLogs.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.MonitoringLogs.ClassName;
    }
}
