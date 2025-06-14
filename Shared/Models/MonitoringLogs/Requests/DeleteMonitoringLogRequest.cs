using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MonitoringLogs.Requests
{
    public class DeleteMonitoringLogRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;
      
        public override string ClassName => StaticClass.MonitoringLogs.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.MonitoringLogs.EndPoint.Delete;
    }
}
