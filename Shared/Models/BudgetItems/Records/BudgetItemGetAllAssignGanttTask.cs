using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.BudgetItems.Records
{
    public class BudgetItemGetAllAssignGanttTask : IGetAll
    {

        public string EndPointName => StaticClass.BudgetItems.EndPoint.GetAllAssignGantTask;
        public Guid ProjectId { get; set; }

    }

}
