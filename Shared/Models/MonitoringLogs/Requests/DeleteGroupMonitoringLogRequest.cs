using Shared.Models.MonitoringLogs.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MonitoringLogs.Requests
{
    public class DeleteGroupMonitoringLogRequest : DeleteMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public override string Legend => "Group of MonitoringLog";

        public override string ClassName => StaticClass.MonitoringLogs.ClassName;

        public HashSet<MonitoringLogResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.MonitoringLogs.EndPoint.DeleteGroup;
    }
}
