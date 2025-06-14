using Shared.Models.MonitoringLogs.Requests;
using Shared.Models.MonitoringLogs.Responses;

namespace Shared.Models.MonitoringLogs.Mappers
{
    public static class MonitoringLogMapper
    {
        public static ChangeMonitoringLogOrderDowmRequest ToDown(this MonitoringLogResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeMonitoringLogOrderUpRequest ToUp(this MonitoringLogResponse response)
        {
            return new()
            {
                ProjectId = response.ProjectId,
                Id = response.Id,
                Name = response.Name,
                Order = response.Order,
            };
        }
       
    }

}
