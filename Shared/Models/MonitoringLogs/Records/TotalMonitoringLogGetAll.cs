using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.MonitoringLogs.Records
{
    public class TotalMonitoringLogGetAll : IGetAll
    {

        public string EndPointName => StaticClass.MonitoringLogs.EndPoint.GetAllTotalProjects;
   
    }
}
