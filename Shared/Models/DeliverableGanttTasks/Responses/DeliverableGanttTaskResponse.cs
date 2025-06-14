using Shared.Enums.TasksRelationTypes;
using Shared.ExtensionsMetods;
using Shared.Models.BudgetItemNewGanttTasks.Responses;
using Shared.Models.DeliverableGanttTasks.Helpers;
using Shared.Models.MainTaskDependencys;

namespace Shared.Models.DeliverableGanttTasks.Responses
{

    public class DeliverableGanttTaskResponse
    {
        public override string ToString()
        {
            return $"{WBS}{Name}";
        }
        public bool IsCreating => Id == Guid.Empty;
        public Guid Id { get; set; } = Guid.Empty;
        public bool IsDeliverable { get; set; } = false;
        public bool IsParentDeliverable { get; set; } = false;
        public bool IsTask => !IsDeliverable;
        public int MainOrder { get; set; } = 0;
        public string MainOrderName => $"{MainOrder} - {Name}";
        public int InternalOrder { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string ParentWBS { get; set; } = string.Empty;
        public string WBS => string.IsNullOrEmpty(ParentWBS) ? $"{InternalOrder}" : $"{ParentWBS}.{InternalOrder}";
        DateTime? _StartDate;
        DateTime? _EndDate;
        public Guid ProjectId { get; set; } = Guid.Empty;
        public string stringStartDate => StartDate == null ? string.Empty : StartDate.Value.ToString("d");
        public string stringEndDate => EndDate == null ? string.Empty : EndDate.Value.ToString("d");
        public bool SetStartDate { get; set; } = false;
        public DateTime? StartDate
        {
            get
            {
                if (IsCalculated)
                {
                    _StartDate = this.GetCalculatedStartDate();
                }


                return _StartDate;
            }
            set
            {
                _StartDate = value;
                if(SetStartDate)
                {
                    this.CalculateDuration();
                }
             
            }
        }
      
        public DateTime? EndDate
        {
            get
            {
                if (HasSubTask)
                {
                    _EndDate = this.GetCalculateEndDate();
                }
                else
                {
                    _EndDate = _StartDate?.AddDays(DurationInDays);
                }
               
                return _EndDate;
            }
            set
            {
                
                _EndDate = value;

            }
        }
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
        public string? DurationUnit { get; set; } = string.Empty;
        public double DurationInDays { get; set; }
        public double DurationInUnit { get; set; }

       
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
     
        public Guid? TaskParentId { get; set; } // Referencia al padre (opcional)
        public Guid DeliverableId { get; set; } // Referencia al deliverable
        public bool HasSubTask => SubTasks.Any();
        public bool HasDependencies => NewDependencies.Where(x => x.DependencyTask != null).Any();
        public bool IsCalculated => HasDependencies || HasSubTask;
        public List<DeliverableGanttTaskResponse> SubTasks { get; set; } = new(); // Colección de subtareas
        public List<DeliverableGanttTaskResponse> OrderedSubTasks => SubTasks.OrderBy(x => x.InternalOrder).ToList();
        public int LastInternalOrder => OrderedSubTasks.Count == 0 ? 0 : OrderedSubTasks.Last().InternalOrder;
        public int LastMainOrder => OrderedSubTasks.Count == 0 ? 0 : OrderedSubTasks.Last().MainOrder;
        public int FirstMainOrder => OrderedSubTasks.Count == 0 ? 0 : OrderedSubTasks.First().MainOrder;
        public List<MainTaskDependencyResponse> NewDependencies { get; set; } = new(); // Colección de dependencias
        public List<MainTaskDependencyResponse> OrderedNewDependencies => NewDependencies.OrderBy(x => x.Order).ToList();
        public List<DeliverableGanttTaskResponse> Dependencies { get; set; } = new();
        public List<BudgetItemNewGanttTaskResponse> BudgetItemGanttTasks { get; set; } = new();
        public List<BudgetItemNewGanttTaskResponse> OrderedBudgetItemGanttTasks => BudgetItemGanttTasks.OrderBy(x => x.Order).ToList();
        public int LastBudgetItemOrder => OrderedBudgetItemGanttTasks.Count == 0 ? 0 : OrderedBudgetItemGanttTasks.Last().Order;
        public int LastDependencyOrder => NewDependencies.Count == 0 ? 0 : OrderedNewDependencies.Last().Order;
        public void RemoveBudgetItem(BudgetItemNewGanttTaskResponse item)
        {
            BudgetItemGanttTasks.Remove(item);
            int neworder = 1;
            foreach (var budget in OrderedBudgetItemGanttTasks)
            {
                budget.Order = neworder;
                neworder++;
            }
        }
        public void RemoveDependency(MainTaskDependencyResponse item)
        {
            NewDependencies.Remove(item);
            int neworder = 1;
            foreach (var dependency in NewDependencies)
            {
                dependency.Order = neworder;
                neworder++;
            }
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
