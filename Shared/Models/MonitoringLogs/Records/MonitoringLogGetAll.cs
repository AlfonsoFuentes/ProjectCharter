using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.MonitoringLogs.Records
{
    public class MonitoringLogGetAll : IGetAll
    {
  
        public string EndPointName => StaticClass.MonitoringLogs.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
    }
}
