using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.MonitoringLogs.Responses
{
    public class MonitoringLogResponseList : IResponseAll
    {
        public List<MonitoringLogResponse> Items { get; set; } = new();
        public Guid ProjectId { get; set; } 
    }
}
