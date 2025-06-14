using Shared.Models.ExpendingTools.Records;
using Shared.Models.ExpendingTools.Responses;
using System.Globalization;

namespace Server.EndPoint.ExpendingTools.Queries
{
    public static class GetAllExpendingToolEndPoint
    {
        static CultureInfo _CurrentCulture = new CultureInfo("en-US");

        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ExpendingTools.EndPoint.GetAll, async (GetAllExpendingTool request, IQueryRepository repository) =>
                    {
                        var cache = StaticClass.ExpendingTools.Cache.GetAll(request.ProjectId);
                        Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x
                         .Include(p => p.BudgetItems).ThenInclude(x => (x as Instrument)!.InstrumentItems!).ThenInclude(x => x.InstrumentTemplate!)
                           
                            .Include(p => p.BudgetItems).ThenInclude(x => (x as Pipe)!.PipeItems!).ThenInclude(x => x.PipeTemplate)
                            .Include(p => p.BudgetItems).ThenInclude(x => (x as Valve)!.ValveItems!).ThenInclude(x => x.ValveTemplate!)
                            .Include(p => p.BudgetItems).ThenInclude(x => (x as Equipment)!.EquipmentItems!).ThenInclude(x => x.EquipmentTemplate!)
                            .Include(x => x.BudgetItems)
                            .ThenInclude(x => x.BudgetItemNewGanttTasks)
                            .ThenInclude(x => x.NewGanttTask)
                            .Include(x => x.BudgetItems)
                            .ThenInclude(x => x.BudgetItemNewGanttTasks)
                            .ThenInclude(x => x.SelectedBasicEngineeringItem!)

                    ;
                        Expression<Func<Project, bool>> criteria = x => x.Id == (request.ProjectId);
                        var project = await repository.GetAsync(Cache: cache, Criteria: criteria, Includes: includes);
                        if (project == null)
                        {
                            return Result<BudgetItemReportDtoList>.Fail(request.NotFound);
                        }
                        if (project.BudgetItems == null || project.BudgetItems.Count == 0)
                        {
                            return Result<BudgetItemReportDtoList>.Fail(request.NotFound);
                        }
                        DateTime currentDate = DateTime.Now;
                        BudgetItemReportDtoList result = new();


                        result.Items = GenerateMonthlyBudgetItemReport(project.BudgetItems, currentDate);




                        return Result<BudgetItemReportDtoList>.Success(result);
                    });

            }
            List<BudgetItemReportDto> GenerateMonthlyBudgetItemReport(List<BudgetItem> budgetItems, DateTime currentDate)
            {

                int currentYear = currentDate.Year;
                int currentMonth = currentDate.Month;
                int previousmonth = currentMonth - 1;
                int firstnexmonth = currentMonth + 1 == 13 ? 12 : currentMonth + 1;

                var budgetItemsWithPurchaseOrders = budgetItems.OrderBy(x => x.OrderList).ToList();
                List<BudgetItemReportDto> result = new List<BudgetItemReportDto>();

                foreach (var budgetItem in budgetItemsWithPurchaseOrders)
                {
                    BudgetItemReportDto dto = new();
                    dto.Name = budgetItem.Name;
                    dto.Order = budgetItem.OrderList;
                    dto.Nomenclatore = budgetItem.Nomenclatore ?? string.Empty;
                    dto.BudgetUSD = budgetItem.BudgetUSD;


                    List<MonthlyData> MonthlyData = new List<MonthlyData>();

                    MonthlyData.Add(GetPreviousYear(budgetItem, currentDate)); // Añadir datos del año anterior

                    MonthlyData.AddRange(GetMonthlyData(budgetItem, currentDate));
                    MonthlyData.Add(GetNextYears(budgetItem, currentDate)); // Añadir datos de años futuros

                    dto.MonthlyData = MonthlyData.OrderBy(x => x.Order).ToList();
                    result.Add(dto);
                }



                return result;
            }
            List<MonthlyData> GetMonthlyData(BudgetItem budgetItem, DateTime currentDate)
            {

                int currentMonth = currentDate.Month;
                int previousmonth = currentMonth - 1;
                int firstnexmonth = currentMonth + 1 == 13 ? 12 : currentMonth + 1;
                int currentYear = currentDate.Year;

                var budgetedAssignedThisMont = budgetItem.NewGanttTasks
                    .Where(x => x.EndDate.Year == currentYear)
                    .GroupBy(x => x.EndDate.Month)
                    .ToDictionary(g => g.Key, g => Math.Round(g.Sum(x => x.TotalBudgetAssigned)));


               

                List<MonthlyData> MonthlyData = new List<MonthlyData>();
                var PreviousMonths = Enumerable.Range(1, previousmonth).Select(monthNum => new MonthlyData
                {
                    Order = monthNum,
                    ColumnName = $"{_CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(monthNum)}-{currentYear}",


                }).ToList();
                var maxmonth = 12 - currentMonth;
                var NextMonths = Enumerable.Range(currentMonth + 1, maxmonth).Select(monthNum => new MonthlyData
                {

                    Order = monthNum,
                    ColumnName = $"{_CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(monthNum)}-{currentYear}",

                }).ToList();
                MonthlyData currentMonthData = new()
                {
                    Order = currentMonth,
                    ColumnName = $"{_CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(currentMonth)}-{currentYear}",
                    BudgetUSD = 0 // Inicializar con 0, se actualizará más adelante
                };



                foreach (var data in PreviousMonths)
                {
                   
                    data.BudgetUSD = budgetedAssignedThisMont.GetValueOrDefault(data.Order, 0);
                }
                MonthlyData.AddRange(PreviousMonths);

               
                currentMonthData.BudgetUSD = budgetedAssignedThisMont.GetValueOrDefault(currentMonthData.Order, 0);

                MonthlyData.Add(currentMonthData);
                if (firstnexmonth != 12)
                {
                    foreach (var data in NextMonths)
                    {

                       
                        data.BudgetUSD = budgetedAssignedThisMont.GetValueOrDefault(data.Order, 0);
                    }
                }



                MonthlyData.AddRange(NextMonths);


                return MonthlyData;
            }

            MonthlyData GetPreviousYear(BudgetItem budgetItem, DateTime currentDate)
            {
                int currentYear = currentDate.Year;
                var budgetedAssignedThisYear = budgetItem.NewGanttTasks
                  .Where(x => x.EndDate.Year < currentYear).Sum(x => x.TotalBudgetAssigned);
                 

                MonthlyData result = new()
                {
                    Order = 0, // Representa el año anterior
                    ColumnName = $"{currentYear - 1}-",
                    BudgetUSD = budgetedAssignedThisYear // Sumar los valores de años anteriores
                };

                return result;
            }
            MonthlyData GetNextYears(BudgetItem budgetItem, DateTime currentDate)
            {
                int currentYear = currentDate.Year;
                var budgetedAssignedThisYear = budgetItem.NewGanttTasks
                 .Where(x => x.EndDate.Year > currentYear).Sum(x => x.TotalBudgetAssigned);

                MonthlyData result = new()
                {
                    Order = 13, // Representa el año anterior
                    ColumnName = $"{currentYear + 1}+",
                    BudgetUSD = budgetedAssignedThisYear // Sumar los valores de años anteriores
                };

                return result;
            }

        }
        
    }
}