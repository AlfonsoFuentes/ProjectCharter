using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.MonitoringTask.Responses
{
    public class MonitoringGanttTaskResponseList : IResponseAll
    {
        public List<MonitoringGanttTaskResponse> Items { get; set; } = new();
        public List<MonitoringGanttTaskResponse> OrderedItems => Items.OrderBy(x => x.MainOrder).ToList();
        public Guid ProjectId {  get; set; }
        public string ProjectName { get; set; }=string.Empty;
        public string CECName { get; set; } = string.Empty;
        public List<MonitoringGanttTaskResponse> GanttTasks => Items.Count == 0 ? new() : Items.Where(x => x.IsTask).ToList();
        public DateTime? ProjectStart => Items.Count == 0 ? null! : OrderedItems.Min(t => t.StartDate);
        public DateTime? ProjectEnd => Items.Count == 0 ? null! : OrderedItems.Max(t => t.EndDate);
    }
}
