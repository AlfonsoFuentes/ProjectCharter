namespace Shared.Models.BudgetItems.Responses
{
    public class BudgetItemResponseAssignGanttTask
    {
        public override string ToString() => $"{CompleteName}";
        public Guid Id { get; set; }
        public Guid? SubId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CompleteName => $"{Nomenclatore} - {Name}";
        public double BudgetUSD { get; set; }
        public string Nomenclatore { get; set; } = string.Empty;
        public int Order { get; set; }
        public string TagNumber { get; set; }=string.Empty;

    }
}
