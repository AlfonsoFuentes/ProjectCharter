using Shared.ExtensionsMetods;
using Shared.Models.BudgetItemNewGanttTasks.Responses;
using Shared.Models.DeliverableGanttTasks.Helpers;
using Shared.Models.MainTaskDependencys;
using Shared.Models.MonitoringTask.Helpers;

namespace Shared.Models.MonitoringTask.Responses
{
    public class MonitoringGanttTaskResponse
    {
        public Guid ProjectId { get; set; }
        public bool IsParentDeliverable { get; set; }
        public string ParentWBS { get; set; } = string.Empty;
        public int InternalOrder { get; set; }
        public Guid? TaskParentId { get; set; } // Referencia al padre (opcional)
        public Guid DeliverableId { get; set; } // Referencia al deliverable
        public Guid Id { get; set; } = Guid.Empty;
        public bool IsDeliverable { get; set; } = false;

        public bool IsTask => !IsDeliverable;
        public List<MainTaskDependencyMonitoringResponse> NewDependencies { get; set; } = new(); // Colección de dependencias
        public List<MonitoringGanttTaskResponse> SubTasks { get; set; } = new(); // Colección de subtareas
        public List<BudgetItemMonitoringNewGanttTaskResponse> BudgetItemGanttTasks { get; set; } = new();

        public DateTime? PlannedStartDate => BudgetItemGanttTasks.Count == 0 ? null : BudgetItemGanttTasks.Min(x => x.PlannedStartDate);

        public DateTime? PlannedEndDate => BudgetItemGanttTasks.Count == 0 ? null : BudgetItemGanttTasks.Max(x => x.PlannedEndDate);
        public double DurationPlannedInDays => PlannedEndDate == null || PlannedStartDate == null ? 0 : (PlannedEndDate - PlannedStartDate)!.Value.Days;

        public int MainOrder { get; set; }
        public string WBS { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Dependencies { get; set; } = string.Empty;
        public bool HasSubTask { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public string stringStartDate => StartDate == null ? string.Empty : StartDate.Value.ToString("d");
        public string stringEndDate => EndDate == null ? string.Empty : EndDate.Value.ToString("d");
        public bool HasDependencies => NewDependencies.Where(x => x.DependencyTask != null).Any();
        public bool IsCalculated => HasDependencies || HasSubTask;
        public string PlannedDuaration {  get; set; }=string.Empty;
        public string? Duration
        {
            get
            {
                if (IsCalculated)
                {
                    this.CalculateDuration();
                }
                return this.GetDuration();

            }
            set
            {
                this.SetDuration(value);

            }
        }
        public double DurationInDays { get; set; }
       
        public double DurationInUnit { get; set; }
        public string? DurationUnit { get; set; } = string.Empty;

        string? _DependencyList;

        public string? DependencyList
        {
            get
            {
                _DependencyList = this.GetDependencyList();
                return _DependencyList;
            }
            set { _DependencyList = value; }
        }
        public double BudgetAssignedUSD => BudgetItemGanttTasks.Sum(x => x.BudgetAssignedUSD);
        public string BudgetAssignedUSDCurrency => BudgetAssignedUSD == 0 ? string.Empty : BudgetAssignedUSD.ToCurrencyCulture();
        private List<string> _textLines = null!;
        public List<string> TextLines(int maxWidth, int averageCharWidth = 7)
        {
            if (_textLines == null)
            {
                _textLines = TextHelper.SplitTextIntoLines($"{WBS} - {Name}", maxWidth, averageCharWidth);
            }
            return _textLines;
        }
    }

}
