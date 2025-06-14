using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Projects.Reponses;
using System.Xml.Linq;

namespace Shared.Models.DeliverableGanttTasks.Responses
{
    public class DeliverableGanttTaskResponseList : IMessageResponse, IResponseAll, IRequest
    {
        public string ProjectName { get; set; } = string.Empty;
        public string EndPointName => StaticClass.DeliverableGanttTasks.EndPoint.UpdateAll;
        public Guid ProjectId { get; set; }
        public string Legend => ProjectName;
        public DateTime? InitialProjectStartDate { get; set; }
        public string ActionType => "updated";
        public string ClassName => StaticClass.DeliverableGanttTasks.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public List<DeliverableGanttTaskResponse> Items { get; set; } = new();
        public List<DeliverableGanttTaskResponse> GanttTasks => Items.Count == 0 ? new() : Items.Where(x => x.IsTask).ToList();
        public List<DeliverableGanttTaskResponse> Deliverables => Items.Count == 0 ? new() : Items.Where(x => x.IsDeliverable).ToList();
        public List<DeliverableGanttTaskResponse> OrderedItems => Items.Count == 0 ? new() : Items.OrderBy(x => x.MainOrder).ToList();
        public int LastMainOrder => Items.Count == 0 ? 0 : OrderedItems.LastOrDefault()?.MainOrder ?? 0;
        public int FirstMainOrder => Items.Count == 0 ? 0 : OrderedItems.FirstOrDefault()?.MainOrder ?? 0;

        public int LastDeliverableOrder => Items.Count == 0 ? 0 : OrderedItems.LastOrDefault(x => x.IsDeliverable)?.MainOrder ?? 0;
        public int FirstDeliverableOrder => OrderedItems.FirstOrDefault(x => x.IsDeliverable)?.MainOrder ?? 0;

        public double maxDuration => Items.Count == 0 ? 0 : Items.Max(x => x.DurationInDays);

        public DateTime? ProjectStart => Items.Count == 0 ? null! : OrderedItems.Min(t => t.StartDate);
        public DateTime? ProjectEnd => Items.Count == 0 ? null! : OrderedItems.Max(t => t.EndDate);
        public double TotalDays => ProjectEnd == null || ProjectStart == null ? 0 : (ProjectEnd!.Value - ProjectStart!.Value)!.TotalDays + 1;
    }
}
