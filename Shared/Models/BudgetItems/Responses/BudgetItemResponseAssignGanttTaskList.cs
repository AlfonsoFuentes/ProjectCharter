using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.BudgetItems.Responses
{
    public class BudgetItemResponseAssignGanttTaskList : IResponseAll
    {
        public List<BudgetItemResponseAssignGanttTask> Items { get; set; } = new List<BudgetItemResponseAssignGanttTask>();
        public List<BudgetItemResponseAssignGanttTask> OrderedItems => Items.OrderBy(x => x.Nomenclatore).ThenBy(x => x.TagNumber).ToList();
    }
}
