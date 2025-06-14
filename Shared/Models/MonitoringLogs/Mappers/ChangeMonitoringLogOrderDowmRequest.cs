using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MonitoringLogs.Mappers
{
    public class ChangeMonitoringLogOrderDowmRequest : UpdateMessageResponse, IRequest
    {

        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.MonitoringLogs.EndPoint.UpdateDown;
        public int Order { get; set; }
        public override string Legend => Name;

        public override string ClassName => StaticClass.MonitoringLogs.ClassName;
    }

}
