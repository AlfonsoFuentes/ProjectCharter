using Server.EndPoint.MonitoringExpendingTools.Queries.BudgetMonitoring;
using Shared.Models.MonitoringExpendingTools.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace Server.EndPoint.MonitoringExpendingTools.Queries
{


    public class BudgetReportService
    {
        private readonly CultureInfo _culture;

        public BudgetReportService(CultureInfo culture) => _culture = culture;

        public List<BudgetItemMonitoringReportDto> Generate(List<BudgetItem> budgetItems, DateTime currentDate)
        {
            return budgetItems
                .OrderBy(b => b.OrderList)
                .Select(b => BuildDto(b, currentDate))
                .ToList();
        }

        private BudgetItemMonitoringReportDto BuildDto(BudgetItem item, DateTime date)
        {
            var dto = new BudgetItemMonitoringReportDto
            {
                Name = item.Name,
                Order = item.OrderList,
                Nomenclatore = item.Nomenclatore ?? string.Empty,
                BudgetUSD = item.BudgetUSD,
                MonthlyData = new(),
                GanttItems = GenerateGanttItems(item)
            };

            dto.GetCompleteData(item, date, _culture);

            return dto;
        }
        public static List<BudgetItemGantMonitoringReportDto> GenerateGanttItems(BudgetItem budgetItem)
        {
            return budgetItem.BudgetItemNewGanttTasks.Count == 0 || budgetItem.BudgetItemNewGanttTasks == null ? new List<BudgetItemGantMonitoringReportDto>() :
                budgetItem.BudgetItemNewGanttTasks.Select(b => new BudgetItemGantMonitoringReportDto
                {
                    BudgetItemId = b.Id,
                    BasicEngineeringItemId = b.SelectedBasicEngineeringItemId,
                    GanttTaskId = b.NewGanttTask.Id,
                    BudgetPlannedUSD = b.NewGanttTask.TotalBudgetAssigned,
                    EndDate = b.NewGanttTask.EndDate,
                    TaskName = b.NewGanttTask.Name,
                    BudgetItemName = budgetItem.Name,

                })
                .ToList();
        }
    }
}