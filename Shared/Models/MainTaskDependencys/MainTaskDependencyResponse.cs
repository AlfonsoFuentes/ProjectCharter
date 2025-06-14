using Shared.Commons;
using Shared.Enums.TasksRelationTypes;
using Shared.Models.DeliverableGanttTasks.Responses;
using System.Text.Json.Serialization;

namespace Shared.Models.MainTaskDependencys
{
    public class MainTaskDependencyResponse
    {
        public int Order { get; set; } = 0;
        [JsonIgnore]
        public DeliverableGanttTaskResponse? MainTask { get; set; } = null!;
        public Guid MainTaskId { get; set; }
        [JsonIgnore]
        public DeliverableGanttTaskResponse? DependencyTask { get; set; } = null!;
        [JsonIgnore]
        public int MainOrder => DependencyTask == null ? 0 : DependencyTask.MainOrder;
        [JsonIgnore]
        public string Name => DependencyTask == null ? string.Empty : DependencyTask.Name;
        public Guid DependencyTaskId { get; set; }
        public TasksRelationTypeEnum DependencyType { get; set; } = TasksRelationTypeEnum.None;
        public string? LagUnit { get; set; } = string.Empty;
        public double LagInDays { get; set; } = 0;
        public double LagInUnits { get; set; } = 0;
        public string? Lag
        {
            get { return this.GetLag(); }
            set { this.SetLag(value); }
        }
        public string GetLag()
        {

            return $"{LagInUnits}{LagUnit}";
        }
        public void SetLag(string? newlag)
        {
            var rawinput = newlag;
            if (string.IsNullOrWhiteSpace(rawinput))
            {
                rawinput = "1d";
            }
            var input = rawinput.Trim();

            var match = System.Text.RegularExpressions.Regex.Match(input, @"^(\d+)\s*([dwmy]?)$");
            if (!match.Success)
            {
                return;
            }
            if (!int.TryParse(match.Groups[1].Value, out var numericValue) || numericValue < 0)
            {
                return;
            }
            LagInUnits = numericValue;
            LagInDays = 0;
            var newlagunits = match.Groups[2].Value.ToLower();
            if (string.IsNullOrEmpty(newlagunits))
            {
                newlagunits = LagUnit;
            }
            LagUnit = newlagunits;
            var newlagdays = 0.0;
            switch (LagUnit)
            {
                case "d":
                    newlagdays = numericValue;
                    break;
                case "w":
                    newlagdays = numericValue * 7;
                    break;
                case "m":
                    newlagdays = numericValue * 30;
                    break;
                case "y":
                    newlagdays = numericValue * 365;
                    break;
                case "":
                    newlagdays = numericValue;

                    break;
                default:
                    newlagdays = numericValue;

                    break;
            }
            LagInDays = newlagdays;

        }
        [JsonIgnore]
        DateTime? _StartDate;
        public DateTime? StartDate
        {
            get
            {
                _StartDate = this.GetStartDate();
                return _StartDate;
            }
        }
        DateTime? _EndDate;
        [JsonIgnore]
        public DateTime? EndDate
        {
            get
            {
                _EndDate = this.GetEndDate();
                return _EndDate;
            }
        }

        public DateTime? GetStartDate()
        {
            DateTime? result = null!;
            if (DependencyTask == null || MainTask == null)
            {
                return null;
            }
            if (DependencyTask.StartDate == null || DependencyTask.EndDate == null) return null;
            switch (DependencyType.Id)
            {
                case 0: //Finish Start La tarea dependiente puede comenzar solo cuando la tarea precedente haya terminado.

                    result = DependencyTask.EndDate!.Value.AddDays(LagInDays + 1);

                    break;
                case 1://Start Start La tarea dependiente puede comenzar solo cuando la tarea precedente haya comenzado

                    result = DependencyTask.StartDate.Value.AddDays(LagInDays);
                    break;

                case 2://Start Finish  La tarea dependiente debe terminar cuando la tarea precedente comience.
                    result = DependencyTask.StartDate!.Value.AddDays(-MainTask.DurationInDays + LagInDays);
                    break;
                case 3://End End  La tarea dependiente debe terminar al mismo tiempo que la tarea precedente.
                    result = DependencyTask.EndDate!.Value.AddDays(-MainTask.DurationInDays + LagInDays);
                    break;

                    //case 3: 
                    //    result = DependencyTask.EndDate!.Value.AddDays(-DependencyTask.DurationInDays);
                    //    break;
            }
            return result;
        }
        public DateTime? GetEndDate()
        {
            DateTime? result = null!;
            if (DependencyTask == null)
            {
                return null;
            }
            if (DependencyTask.StartDate == null || DependencyTask.EndDate == null) return null;
            switch (DependencyType.Id)
            {
                case 0: //Finish Start La tarea dependiente puede comenzar solo cuando la tarea precedente haya terminado.
                case 1://Start Start La tarea dependiente puede comenzar solo cuando la tarea precedente haya comenzado
                case 2: //Start Finish  La tarea dependiente debe terminar cuando la tarea precedente comience
                case 3: //End End  La tarea dependiente debe terminar al mismo tiempo que la tarea precedente..
                    result = _StartDate!.Value.AddDays(DependencyTask.DurationInDays);
                    break;

                    //    result = StartDate!.Value.AddDays(DependencyTask.DurationInDays);

                    //    break;

                    //case 2: //Start Finish  La tarea dependiente debe terminar cuando la tarea precedente comience.
                    //    DateTime? MinDateFromDependencies = row.Dependencies.Min(x => x.StartDate);
                    //    result = MinDateFromDependencies!.Value.AddDays(row.LagInDays);
                    //    break;

                    //case 3: //End End  La tarea dependiente debe terminar al mismo tiempo que la tarea precedente.
                    //    DateTime? MaxDateFromDependencies = row.Dependencies.Max(x => x.EndDate);
                    //    result = MaxDateFromDependencies!.Value.AddDays(row.LagInDays);
                    //    break;
            }
            return result;
        }

    }
}
