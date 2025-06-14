using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.Requests
{
    public class MoveBudgetItemGroupRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = "Move Group BudgetItem";
        public override string Legend => Name;

        public override string ClassName => StaticClass.BudgetItems.ClassName;

        public Guid ProjectId { get; set; }
        public List<Guid> MoveGroup { get; set; } = new();
        public Guid MilestoneId { get; set; }
        public string EndPointName => StaticClass.BudgetItems.EndPoint.MoveGroup;


    }
}
