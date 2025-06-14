using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.Requests
{
    public class UpdateBudgetItemGanntTaskRequest : UpdateMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.BudgetItems.ClassName;
        public Guid GanttTaskId { get; set; }
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.BudgetItems.EndPoint.UpdateGanttTask;


    }
}
