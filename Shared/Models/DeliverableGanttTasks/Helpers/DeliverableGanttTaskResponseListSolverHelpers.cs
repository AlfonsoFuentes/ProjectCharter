using Shared.Enums.TasksRelationTypes;
using Shared.Models.DeliverableGanttTasks.Responses;
using System.Text;

namespace Shared.Models.DeliverableGanttTasks.Helpers
{
    public static class DeliverableGanttTaskResponseListSolverHelpers
    {
        public static DateTime? GetCalculatedStartDate(this DeliverableGanttTaskResponse row)
        {
            DateTime? result = DateTime.MinValue;
            if (row.HasSubTask)
            {
                result = row.SubTasks.Min(x => x.StartDate);
            }
            else if (row.HasDependencies)
            {
                result = row.NewDependencies.Max(x => x.StartDate);

            }
            return result;
        }
        public static DateTime? GetCalculateEndDate(this DeliverableGanttTaskResponse row)
        {
            DateTime? result = DateTime.MinValue;
            if (row.HasSubTask)
            {
                result = row.SubTasks.Max(x => x.EndDate);
            }
            else if (row.HasDependencies && row.StartDate != null)
            {
                result = row.StartDate!.Value.AddDays(row.DurationInDays + 1);// row.NewDependencies.Max(x => x.EndDate);

            }
            return result;
        }
        public static void CalculateDuration(this DeliverableGanttTaskResponse row)
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
        public static void CalculateDurationWithEndDate(this DeliverableGanttTaskResponse row, DateTime? enddate)
        {
            if (row.StartDate.HasValue && enddate.HasValue)
            {
                row.DurationInDays = (enddate.Value - row.StartDate.Value).TotalDays;
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

        public static string GetDuration(this DeliverableGanttTaskResponse row)
        {

            return $"{row.DurationInUnit}{row.DurationUnit}";
        }

        public static void SetDuration(this DeliverableGanttTaskResponse task, string? newduration)
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
        public static string SetDependeyList(this DeliverableGanttTaskResponseList response, DeliverableGanttTaskResponse row, string dependencylistFromUI)
        {

            row.Dependencies.Clear();
            row.DependencyList = string.Empty;
            if (string.IsNullOrEmpty(dependencylistFromUI))
                return string.Empty;
            var dependencyList = dependencylistFromUI.Split(',')
                                               .Select(dep => dep.Trim())
                                               .Where(dep => !string.IsNullOrEmpty(dep))
                                               .ToList();


            StringBuilder result = new StringBuilder();
            StringBuilder resultMessages = new StringBuilder();
            foreach (var dependency in dependencyList)
            {
                var publisher = response.GetRowFromMainOrder(dependency);
                if (publisher != null)
                {
                    var resultDependency = response.IsValidNewDependency(row, publisher);
                    if (!string.IsNullOrEmpty(resultDependency))
                    {
                        Console.WriteLine(resultDependency);
                        resultMessages.Append(resultDependency);
                        return resultMessages.ToString();
                    }
                    else
                    {
                        row.Dependencies.Add(publisher);

                        if (result.Length > 0)
                        {
                            result.Append(", ");
                        }
                        result.Append($"{dependency}");

                    }
                }


            }
            row.DependencyList = result.ToString();
            return resultMessages.ToString();
        }
        public static string GetDependencyList(this DeliverableGanttTaskResponse row)
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
                    if (dependency.LagInDays>0)
                    {
                        result.Append($"+{dependency.Lag}");
                    }
                }

            }
            return result.ToString();
        }


    }
}
