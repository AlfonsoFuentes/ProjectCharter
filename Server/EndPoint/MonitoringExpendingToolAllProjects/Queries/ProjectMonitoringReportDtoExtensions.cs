using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Enums.PurchaseOrderStatusEnums;
using Shared.Models.MonitoringExpendingTools.Responses;
using System.Globalization;

namespace Server.EndPoint.MonitoringExpendingToolAllProjects.Queries
{
    public static class ProjectMonitoringReportDtoExtensions
    {
        public static void GetCompleteData(this ProjectMonitoringReportDto dto, Project item, DateTime date, CultureInfo culture)
        {
            dto.GetPreviousYear(item, date, culture);
            dto.GetMonthlyData(item, date, culture);

            dto.GetNextYears(item, date, culture);
        }


        public static void GetMonthlyData(this ProjectMonitoringReportDto dto, Project item, DateTime date, CultureInfo culture)
        {
            DateTime QueryDate = DateTime.Now;
            int currentYear = date.Year;
            int previousMonthsCount = date.Month - 1;
            var OrdenesdeCompra = item.CapitalPurchaseOrders;
            for (int i = 1; i <= 12; i++)
            {
                MonthlyMonitoringData monthData = new()
                {
                    Order = i,
                    ColumnName = $"{culture.DateTimeFormat.GetAbbreviatedMonthName(i)}-{currentYear}",
                    ValueUSD = 0,
                    Month = i,
                    Year = currentYear
                };
                dto.MonthlyData.Add(monthData);
                var recibidasEsteMes = OrdenesdeCompra

                   .SelectMany(po => po.PurchaseOrderItems)
                   .SelectMany(poi => poi.PurchaseOrderReceiveds)
                   .Where(r => r.CurrencyDate.HasValue && r.CurrencyDate.Value.Year == currentYear && r.CurrencyDate.Value.Month == i)
                   .ToList().Sum(x => x.ReceivedUSD);
                monthData.ValueUSD = recibidasEsteMes;
                monthData.Actuals = OrdenesdeCompra.Where(x => x.PurchaseOrderItems
                 .Any(y => y.PurchaseOrderReceiveds
                 .Any(z => z.CurrencyDate.HasValue && z.CurrencyDate.Value.Year == currentYear &&
                          z.CurrencyDate.Value.Month == i))).Select(selector => selector.Map()).ToList();


                if (i >= QueryDate.Month)
                {

                    var esperadasEsteMesList = OrdenesdeCompra
                        .Where(x => x.PurchaseOrderStatus != PurchaseOrderStatusEnum.Closed.Id && x.ExpectedDate.HasValue &&
                                     x.ExpectedDate.Value.Year == currentYear && x.ExpectedDate.Value.Month == i)
                        .SelectMany(po => po.PurchaseOrderItems).ToList();

                    var esperadasEsteMes = esperadasEsteMesList.Sum(x => x.CommitmentItemUSD);

                    monthData.Commitmments = OrdenesdeCompra
                        .Where(x => x.PurchaseOrderStatus != PurchaseOrderStatusEnum.Closed.Id && x.ExpectedDate.HasValue &&
                                     x.ExpectedDate.Value.Year == currentYear && x.ExpectedDate.Value.Month == i ).Select(x => x.Map()).ToList();

                    monthData.ValueUSD += esperadasEsteMes;
                    if (esperadasEsteMesList.Count == 0)
                    {
                        var ganttTasksAssigned = dto.GanttItems.Where(x => x.EndDate.Year == currentYear && x.EndDate.Month == i).ToList();
                        monthData.ValueUSD += ganttTasksAssigned.Sum(x => x.TaskPendingBudgetUSD);
                    }

                }

            }


        }

        static void GetPreviousYear(this ProjectMonitoringReportDto dto, Project project, DateTime currentDate, CultureInfo culture)
        {
            int currentYear = currentDate.Year;

            // Solo se consideran las órdenes recibidas en años anteriores
            var receivedPriorYears = project.CapitalPurchaseOrders.SelectMany(y => y.PurchaseOrderItems)
                .SelectMany(poi => poi.PurchaseOrderReceiveds)
                .Where(r => r.CurrencyDate.HasValue && r.CurrencyDate.Value.Year < currentYear)
                .Sum(r => Math.Round(r.ReceivedUSD, 2));

            MonthlyMonitoringData result = new()
            {
                Order = 0, // Representa datos del año anterior o históricos
                ColumnName = $"{currentYear - 1}-", // Formato "Año-"
                ValueUSD = receivedPriorYears,

                Month = -1,
                Year = currentYear - 1,
            };

            dto.MonthlyData.Add(result);
        }
        static void GetNextYears(this ProjectMonitoringReportDto dto, Project Item, DateTime currentDate, CultureInfo culture)
        {
            int currentYear = currentDate.Year;

            var OrdenesdeCompra = Item.CapitalPurchaseOrders;

            MonthlyMonitoringData nextYearData = new()
            {
                Order = 13,
                ColumnName = $"{currentYear + 1}+",
                ValueUSD = 0,
                Month = -1,
                Year = currentYear + 1

            };

            dto.MonthlyData.Add(nextYearData);

            var esperadasMayoresAEsteAño = OrdenesdeCompra.SelectMany(po => po.PurchaseOrderItems)
                .Where(poi => poi.PurchaseOrder != null && poi.PurchaseOrder.ExpectedDate.HasValue &&
                              poi.PurchaseOrder.ExpectedDate.Value.Year > currentYear)
                .ToList().Sum(x => x.CommitmentItemUSD);
            nextYearData.ValueUSD = esperadasMayoresAEsteAño;
            var ganttTasksAssigned = dto.GanttItems.Where(x => x.EndDate.Year > currentYear).ToList();
            foreach (var ganttTask in ganttTasksAssigned)
            {
                var ordenesDeCompraEsperadaPorBasicItem = OrdenesdeCompra.SelectMany(po => po.PurchaseOrderItems)
                       .Where(poi => poi.BudgetItemId == ganttTask.BudgetItemId &&
                                        poi.BudgetItemId == ganttTask.BudgetItemId &&
                                     poi.PurchaseOrder != null && poi.PurchaseOrder.ExpectedDate.HasValue &&
                                     poi.PurchaseOrder.ExpectedDate.Value.Year > currentYear
                                    )
                       .ToList().Sum(x => x.CommitmentItemUSD);

                ganttTask.AssignedUSD += ordenesDeCompraEsperadaPorBasicItem /*+ ordenesDeCompraRecibidasPorBasicItem*/;
                if (ganttTask.EndDate.Year > currentYear)
                {
                    nextYearData.ValueUSD += ganttTask.TaskPendingBudgetUSD;

                }
            }



        }
    }
}