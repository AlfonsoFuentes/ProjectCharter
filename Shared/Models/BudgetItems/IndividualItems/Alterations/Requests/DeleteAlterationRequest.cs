using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Alterations.Requests
{
    public class DeleteAlterationRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;
        public Guid ProjectId { get; set; }
        public override string ClassName => StaticClass.Alterations.ClassName;
        public Guid? GanttTaskId { get; set; }
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Alterations.EndPoint.Delete;


    }
}
