using Shared.Enums.TasksRelationTypes;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.MonitoringTask.Responses;
using System.Text;

namespace Shared.Models.MonitoringTask.Helpers
{
    public static class MonitorinGanttTaskHelpers
    {
        public static void UpdateSubTaskAndDependencies(this MonitoringGanttTaskResponseList response)
        {
            response.UpdateSubTasks();

            response.UpdateNewDependencies();
        }
        private static void UpdateSubTasks(this MonitoringGanttTaskResponseList response)
        {
            var flatList = response.Items.ToList();

            var deliverables = response.Items.Where(x => x.IsDeliverable).ToList();
            var idToItemMap = flatList.ToDictionary(item => item.Id);



            foreach (var deliverable in deliverables)
            {
                var tasks = flatList.Where(x => x.IsTask && x.DeliverableId == deliverable.Id).ToList();
                foreach (var item in tasks)
                {

                    if (item.IsParentDeliverable && item.TaskParentId == deliverable.Id)
                    {
                        deliverable.SubTasks.Add(item);

                    }
                    else if (item.TaskParentId.HasValue && idToItemMap.TryGetValue(item.TaskParentId.Value, out var parent))
                    {
                        parent.SubTasks.Add(item);
                    }
                }
            }


        }

        private static void UpdateNewDependencies(this MonitoringGanttTaskResponseList response)
        {
            response.GanttTasks.ForEach(task =>
            {
                task.NewDependencies.ForEach(dependency =>
                {
                    dependency.DependencyTask = response.GanttTasks.FirstOrDefault(x => x.Id == dependency.DependencyTaskId);
                    dependency.MainTask = response.GanttTasks.FirstOrDefault(x => x.Id == dependency.MainTaskId);
                });
            });
        }
        public static void UpdateBudgetItems(this MonitoringGanttTaskResponseList response, BudgetItemWithPurchaseOrderResponseList BudgetItemResponse)
        {
            response.GanttTasks.ForEach(task =>
            {
                task.BudgetItemGanttTasks.ForEach(budget =>
                {
                    budget.BudgetItem = BudgetItemResponse.OrderedItems.FirstOrDefault(x => x.Id == budget.BudgetItemId);
                });
            });

        }
        public static string GetDependencyList(this MonitoringGanttTaskResponse row)
        {
            if (row.NewDependencies.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder result = new StringBuilder();
            foreach (var dependency in row.NewDependencies)
            {
                if (dependency.DependencyTask != null)
                {
                    if (result.Length > 0)
                    {
                        result.Append(", ");
                    }
                    result.Append($"{dependency.MainOrder}");
                    if (dependency.DependencyType.Id != TasksRelationTypeEnum.FinishStart.Id)
                    {
                        result.Append($"{dependency.DependencyType.Name}");
                    }
                    if (dependency.LagInDays > 0)
                    {
                        result.Append($"+{dependency.Lag}");
                    }
                }

            }
            return result.ToString();
        }
        public static void CalculateDuration(this MonitoringGanttTaskResponse row)
        {
            if (row.StartDate.HasValue && row.EndDate.HasValue)
            {
                row.DurationInDays = (row.EndDate.Value - row.StartDate.Value).TotalDays;
            }
            else
            {
                row.DurationInDays = 0;
            }
            switch (row.DurationUnit)
            {
                case "d":
                    row.DurationInUnit = row.DurationInDays;
                    break;
                case "w":
                    row.DurationInUnit = Math.Round(row.DurationInDays / 7, 1);
                    break;
                case "m":
                    row.DurationInUnit = Math.Round(row.DurationInDays / 30, 1);
                    break;
                case "y":
                    row.DurationInUnit = Math.Round(row.DurationInDays / 365, 1);
                    break;
                case "":
                    row.DurationInUnit = row.DurationInDays;

                    break;
                default:
                    row.DurationInUnit = row.DurationInDays;

                    break;
            }
        }
        public static void CalculatePlannedDuration(this MonitoringGanttTaskResponse row)
        {
            double duration = 0;
            if (row.PlannedStartDate == null || row.PlannedEndDate == null) return;

            if (row.StartDate.HasValue && row.EndDate.HasValue)
            {
                duration = (row.PlannedEndDate.Value - row.PlannedStartDate.Value).TotalDays;
            }
           
            switch (row.DurationUnit)
            {
                case "d":
                    row.DurationInUnit = row.DurationInDays;
                    break;
                case "w":
                    row.DurationInUnit = Math.Round(row.DurationInDays / 7, 1);
                    break;
                case "m":
                    row.DurationInUnit = Math.Round(row.DurationInDays / 30, 1);
                    break;
                case "y":
                    row.DurationInUnit = Math.Round(row.DurationInDays / 365, 1);
                    break;
                case "":
                    row.DurationInUnit = row.DurationInDays;

                    break;
                default:
                    row.DurationInUnit = row.DurationInDays;

                    break;
            }
        }
        public static string GetDuration(this MonitoringGanttTaskResponse row)
        {

            return $"{row.DurationInUnit}{row.DurationUnit}";
        }
        public static void SetDuration(this MonitoringGanttTaskResponse task, string? newduration)
        {
            var rawinput = newduration;
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
            task.DurationInUnit = numericValue;

            var newdurationunits = match.Groups[2].Value.ToLower();
            if (string.IsNullOrEmpty(newdurationunits))
            {
                newdurationunits = task.DurationUnit;
            }
            task.DurationUnit = newdurationunits;

            switch (task.DurationUnit)
            {
                case "d":
                    task.DurationInDays = numericValue;
                    break;
                case "w":
                    task.DurationInDays = numericValue * 7;
                    break;
                case "m":
                    task.DurationInDays = numericValue * 30;
                    break;
                case "y":
                    task.DurationInDays = numericValue * 365;
                    break;
                case "":
                    task.DurationInDays = numericValue;

                    break;
                default:
                    task.DurationInDays = numericValue;

                    break;
            }


        }
    }
}
