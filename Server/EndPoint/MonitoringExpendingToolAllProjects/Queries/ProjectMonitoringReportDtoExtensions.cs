using Shared.Models.MonitoringExpendingTools.Responses;
using Server.Database.Entities.PurchaseOrders;
using System.Globalization;

namespace Server.EndPoint.MonitoringExpendingTools.Queries
{
    public static class ProjectMonitoringReportDtoExtensions
    {
        public static void GetCompleteData(this ProjectMonitoringReportDto dto, Project item, DateTime date, CultureInfo culture)
        {
            dto.GetPreviousYear(item, date, culture);
            dto.GetMonthlyData(item, date, culture);

            dto.GetNextYears(item, date, culture);
        }
        public static void GetMonthlyData2(this ProjectMonitoringReportDto dto, Project item, DateTime date, CultureInfo culture)
        {
            int currentYear = date.Year;
            int previousMonthsCount = date.Month - 1;
            var OrdenesdeCompra = item.PurchaseOrders;
            for (int i = 1; i <= 12; i++)
            {
                MonthlyMonitoringData monthData = new()
                {
                    Order = i,
                    ColumnName = $"{culture.DateTimeFormat.GetAbbreviatedMonthName(i)}-{currentYear}",
                    ValueUSD = 0,

                };
                dto.MonthlyData.Add(monthData);
                var recibidasEsteMes = OrdenesdeCompra.SelectMany(po => po.PurchaseOrderItems)
                    .SelectMany(poi => poi.PurchaseOrderReceiveds)
                    .Where(r => r.CurrencyDate.HasValue && r.CurrencyDate.Value.Year == currentYear && r.CurrencyDate.Value.Month == i)
                    .ToList().Sum(x => x.ReceivedUSD);
                monthData.ValueUSD = recibidasEsteMes;
                var esperadasEsteMes = OrdenesdeCompra.SelectMany(po => po.PurchaseOrderItems)
                    .Where(poi => poi.PurchaseOrder != null && poi.PurchaseOrder.ExpectedDate.HasValue &&
                                  poi.PurchaseOrder.ExpectedDate.Value.Year == currentYear && poi.PurchaseOrder.ExpectedDate.Value.Month == i)
                    .ToList().Sum(x => x.CommitmentItemUSD);
                monthData.ValueUSD += esperadasEsteMes;
                foreach (var ganttTask in dto.GanttItems)
                {
                    var ordenesDeCompraEsperadaPorBasicItem = OrdenesdeCompra.SelectMany(po => po.PurchaseOrderItems)
                           .Where(poi => poi.BasicEngineeringItemId == ganttTask.BasicEngineeringItemId &&
                                         poi.PurchaseOrder != null && poi.PurchaseOrder.ExpectedDate.HasValue &&
                                         poi.PurchaseOrder.ExpectedDate.Value.Year == currentYear &&
                                         poi.PurchaseOrder.ExpectedDate.Value.Month == i)
                           .ToList().Sum(x => x.CommitmentItemUSD);
                    var ordenesDeCompraRecibidasPorBasicItem = OrdenesdeCompra.SelectMany(po => po.PurchaseOrderItems)
                       .Where(r => r.BasicEngineeringItemId == ganttTask.BasicEngineeringItemId)
                       .SelectMany(x => x.PurchaseOrderReceiveds.Where(x => x.CurrencyDate.HasValue && x.CurrencyDate.Value.Month == i && x.CurrencyDate.Value.Year == currentYear))
                        .ToList().Sum(x => x.ReceivedUSD);
                    ganttTask.AssignedUSD += ordenesDeCompraEsperadaPorBasicItem + ordenesDeCompraRecibidasPorBasicItem;
                    if (ganttTask.EndDate.Month == i && ganttTask.EndDate.Year == currentYear)
                    {
                        var OrdenesdeCompraEsperadasLosSiguientesMeses = OrdenesdeCompra.SelectMany(po => po.PurchaseOrderItems)
                           .Where(poi => poi.BasicEngineeringItemId == ganttTask.BasicEngineeringItemId &&
                                         poi.PurchaseOrder != null && poi.PurchaseOrder.ExpectedDate.HasValue &&
                                         poi.PurchaseOrder.ExpectedDate.Value.Year == currentYear &&
                                         poi.PurchaseOrder.ExpectedDate.Value.Month > i)
                           .ToList();
                        var ordenesdeCompraRecibidasLosSiguientesMeses = OrdenesdeCompra.SelectMany(po => po.PurchaseOrderItems)
                            .Where(r => r.BasicEngineeringItemId == ganttTask.BasicEngineeringItemId)
                            .SelectMany(x => x.PurchaseOrderReceiveds.Where(x => x.CurrencyDate.HasValue && x.CurrencyDate.Value.Month > i && x.CurrencyDate.Value.Year == currentYear))
                            .ToList();
                        if (OrdenesdeCompraEsperadasLosSiguientesMeses.Count == 0 && ordenesdeCompraRecibidasLosSiguientesMeses.Count == 0)
                        {
                            monthData.ValueUSD += ganttTask.TaskPendingBudgetUSD;
                        }
                    }
                }
            }


        }


        public static void GetMonthlyData(this ProjectMonitoringReportDto dto, Project item, DateTime date, CultureInfo culture)
        {
            int currentYear = date.Year;
            int previousMonthsCount = date.Month - 1;
            var OrdenesdeCompra = item.PurchaseOrders;
            for (int i = 1; i <= 12; i++)
            {
                MonthlyMonitoringData monthData = new()
                {
                    Order = i,
                    ColumnName = $"{culture.DateTimeFormat.GetAbbreviatedMonthName(i)}-{currentYear}",
                    ValueUSD = 0,

                };
                dto.MonthlyData.Add(monthData);
                var recibidasEsteMes = OrdenesdeCompra.SelectMany(po => po.PurchaseOrderItems)
                    .SelectMany(poi => poi.PurchaseOrderReceiveds)
                    .Where(r => r.CurrencyDate.HasValue && r.CurrencyDate.Value.Year == currentYear && r.CurrencyDate.Value.Month == i)
                    .ToList().Sum(x => x.ReceivedUSD);
                monthData.ValueUSD = recibidasEsteMes;
                var esperadasEsteMes = OrdenesdeCompra.SelectMany(po => po.PurchaseOrderItems)
                    .Where(poi => poi.PurchaseOrder != null && poi.PurchaseOrder.ExpectedDate.HasValue &&
                                  poi.PurchaseOrder.ExpectedDate.Value.Year == currentYear && poi.PurchaseOrder.ExpectedDate.Value.Month == i)
                    .ToList().Sum(x => x.CommitmentItemUSD);
                monthData.ValueUSD += esperadasEsteMes;
                foreach (var ganttTask in dto.GanttItems)
                {
                    var ordenesDeCompraEsperadaPorBasicItem = OrdenesdeCompra.SelectMany(po => po.PurchaseOrderItems)
                           .Where(poi => poi.BasicEngineeringItemId == ganttTask.BasicEngineeringItemId &&
                           poi.BudgetItemId == ganttTask.BudgetItemId &&
                                         poi.PurchaseOrder != null && poi.PurchaseOrder.ExpectedDate.HasValue &&
                                         poi.PurchaseOrder.ExpectedDate.Value.Year == currentYear &&
                                         poi.PurchaseOrder.ExpectedDate.Value.Month == i)
                           .ToList().Sum(x => x.CommitmentItemUSD);
                    var ordenesDeCompraRecibidasPorBasicItem = OrdenesdeCompra.SelectMany(po => po.PurchaseOrderItems)
                       .Where(r => r.BasicEngineeringItemId == ganttTask.BasicEngineeringItemId &&
                           r.BudgetItemId == ganttTask.BudgetItemId)
                       .SelectMany(x => x.PurchaseOrderReceiveds.Where(x => x.CurrencyDate.HasValue && x.CurrencyDate.Value.Month == i && x.CurrencyDate.Value.Year == currentYear))
                        .ToList().Sum(x => x.ReceivedUSD);
                    ganttTask.AssignedUSD += ordenesDeCompraEsperadaPorBasicItem + ordenesDeCompraRecibidasPorBasicItem;
                    if (ganttTask.EndDate.Month == i && ganttTask.EndDate.Year == currentYear)
                    {
                        var OrdenesdeCompraEsperadasLosSiguientesMeses = OrdenesdeCompra.SelectMany(po => po.PurchaseOrderItems)
                           .Where(poi => poi.BasicEngineeringItemId == ganttTask.BasicEngineeringItemId &&
                           poi.BudgetItemId == ganttTask.BudgetItemId &&
                                         poi.PurchaseOrder != null && poi.PurchaseOrder.ExpectedDate.HasValue &&
                                         poi.PurchaseOrder.ExpectedDate.Value.Year == currentYear &&
                                         poi.PurchaseOrder.ExpectedDate.Value.Month > i)
                           .ToList();
                        var ordenesdeCompraRecibidasLosSiguientesMeses = OrdenesdeCompra.SelectMany(po => po.PurchaseOrderItems)
                            .Where(r => r.BasicEngineeringItemId == ganttTask.BasicEngineeringItemId && r.BudgetItemId == ganttTask.BudgetItemId)
                            .SelectMany(x => x.PurchaseOrderReceiveds.Where(x => x.CurrencyDate.HasValue && x.CurrencyDate.Value.Month > i && x.CurrencyDate.Value.Year == currentYear))
                            .ToList();
                        if (OrdenesdeCompraEsperadasLosSiguientesMeses.Count == 0 && ordenesdeCompraRecibidasLosSiguientesMeses.Count == 0)
                        {
                            monthData.ValueUSD += ganttTask.TaskPendingBudgetUSD;
                        }
                    }
                }
            }


        }

        static void GetPreviousYear(this ProjectMonitoringReportDto dto, Project project, DateTime currentDate, CultureInfo culture)
        {
            int currentYear = currentDate.Year;

            // Solo se consideran las órdenes recibidas en años anteriores
            var receivedPriorYears = project.PurchaseOrders.SelectMany(y => y.PurchaseOrderItems)
                .SelectMany(poi => poi.PurchaseOrderReceiveds)
                .Where(r => r.CurrencyDate.HasValue && r.CurrencyDate.Value.Year < currentYear)
                .Sum(r => Math.Round(r.ReceivedUSD, 2));

            MonthlyMonitoringData result = new()
            {
                Order = 0, // Representa datos del año anterior o históricos
                ColumnName = $"{currentYear - 1}-", // Formato "Año-"
                ValueUSD = receivedPriorYears,
                //ValueByPOUSD = receivedPriorYears // Si solo vienen de POs recibidos
            };

            dto.MonthlyData.Add(result);
        }
        static void GetNextYears(this ProjectMonitoringReportDto dto, Project Item, DateTime currentDate, CultureInfo culture)
        {
            int currentYear = currentDate.Year;

            var OrdenesdeCompra = Item.PurchaseOrders;

            MonthlyMonitoringData nextYearData = new()
            {
                Order = 13,
                ColumnName = $"{currentYear + 1}+",
                ValueUSD = 0,

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
                       .Where(poi => poi.BasicEngineeringItemId == ganttTask.BasicEngineeringItemId &&
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