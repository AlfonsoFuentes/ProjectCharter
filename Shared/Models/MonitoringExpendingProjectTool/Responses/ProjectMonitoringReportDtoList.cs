﻿using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.MonitoringExpendingTools.Responses
{
    public class ProjectMonitoringReportDtoList : IResponseAll
    {
        public List<ProjectMonitoringReportDto> Items { get; set; } = new List<ProjectMonitoringReportDto>();

        public double BudgetUSD => Items.Count == 0 ? 0 : Items.Sum(x => x.BudgetUSD);
        public double PendingBudgetUSD => Items.Count == 0 ? 0 : Items.Sum(x => x.PendingBudgetUSD);
        public List<ProjectMonitoringReportDto> OrderedItems => Items.Count == 0 ? new() :
           Items.OrderBy(x => x.ProjectNumber).ToList();
        public List<ColumnMonitoringName> Columns => OrderedItems.Count == 0 ? new() : OrderedItems[0].MonthlyData.Select(x => new ColumnMonitoringName()
        {
            Order = x.Order,
            Name = x.ColumnName,
            ValueUSD = OrderedItems.Sum(y => y.MonthlyData.Where(z => z.Order == x.Order).Sum(z => z.ValueUSD))
        }).ToList();
        public List<ColumnMonitoringName> OrderedColumns => Columns.Count == 0 ? new() : Columns.OrderBy(x => x.Order).ToList();

    }
}
