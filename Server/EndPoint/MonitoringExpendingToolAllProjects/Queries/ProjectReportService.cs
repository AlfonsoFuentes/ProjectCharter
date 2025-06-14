using Server.EndPoint.MonitoringExpendingTools.Queries;
using Server.EndPoint.MonitoringExpendingTools.Queries.BudgetMonitoring;
using Shared.Models.MonitoringExpendingTools.Responses;
using System.Globalization;
namespace Server.EndPoint.MonitoringExpendingToolAllProjects.Queries
{
    public class ProjectReportService
    {
        private readonly CultureInfo _culture;

        public ProjectReportService(CultureInfo culture) => _culture = culture;

        public List<ProjectMonitoringReportDto> Generate(List<Project> budgetItems, DateTime currentDate)
        {
            return budgetItems
                .OrderBy(b => b.ProjectNumber)
                .Select(b => BuildDto(b, currentDate))
                .ToList();
        }

        private ProjectMonitoringReportDto BuildDto(Project item, DateTime date)
        {
            var dto = new ProjectMonitoringReportDto
            {
                Name = item.Name,
                ProjectNumber = item.ProjectNumber ?? string.Empty,

                BudgetUSD = item.BudgetItems.Sum(x => x.BudgetUSD),
                MonthlyData = new(),
                GanttItems = GenerateGanttItems(item),
            };

            dto.GetCompleteData(item, date, _culture);

            return dto;
        }
        public List<BudgetItemGantMonitoringReportDto> GenerateGanttItems(Project item)
        {
            var ganttTask = item.BudgetItems.SelectMany(b => b.BudgetItemNewGanttTasks.Where(x=>x.GanttTaskBudgetAssigned>0).ToList());

            var result = ganttTask.Select(b => new BudgetItemGantMonitoringReportDto
            {
                BudgetItemId = b.BudgetItem.Id,
                BasicEngineeringItemId = b.SelectedBasicEngineeringItemId,
                GanttTaskId = b.NewGanttTask.Id,
                BudgetPlannedUSD = b.NewGanttTask.TotalBudgetAssigned,
                EndDate = b.NewGanttTask.EndDate,
                TaskName = b.NewGanttTask.Name,
                BudgetItemName = item.Name,
            }).ToList();

           
            return result;
        }
    }
}