using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MonitoringLogs.Records
{
   public class GetMonitoringLogByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid ProjectId { get; set; }

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.MonitoringLogs.EndPoint.GetById;
        public override string ClassName => StaticClass.MonitoringLogs.ClassName;
    }

}
