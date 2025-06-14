using Shared.Models.ExpendingTools.Records;
using Shared.Models.ExpendingTools.Responses;

namespace Server.EndPoint.ExpendingTools.Queries
{
    //public static class GetAllExpendingToolEndPoint2
    //{


    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.ExpendingTools.EndPoint.GetAll,
    //                async (GetAllExpendingTool request, IQueryRepository repository) =>
    //                {
    //                    var cache = StaticClass.ExpendingTools.Cache.GetAll(request.ProjectId);
    //                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x
    //                     .Include(p => p.BudgetItems).ThenInclude(x => (x as Instrument)!.InstrumentItems!).ThenInclude(x => x.InstrumentTemplate!)

    //                        .Include(p => p.BudgetItems).ThenInclude(x => (x as Pipe)!.PipeItems!).ThenInclude(x => x.PipeTemplate)
    //                        .Include(p => p.BudgetItems).ThenInclude(x => (x as Valve)!.ValveItems!).ThenInclude(x => x.ValveTemplate!)
    //                        .Include(p => p.BudgetItems).ThenInclude(x => (x as Equipment)!.EquipmentItems!).ThenInclude(x => x.EquipmentTemplate!)
    //                        .Include(x => x.BudgetItems)
    //                        .ThenInclude(x => x.BudgetItemNewGanttTasks)
    //                        .ThenInclude(x => x.NewGanttTask)

    //                ;
    //                    Expression<Func<Project, bool>> criteria = x => x.Id == (request.ProjectId);
    //                    var project = await repository.GetAsync(Cache: cache, Criteria: criteria, Includes: includes);
    //                    if (project == null)
    //                    {
    //                        return Result<ExpendingToolResponseList>.Fail(request.NotFound);
    //                    }
    //                    if (project.BudgetItems == null || project.BudgetItems.Count == 0)
    //                    {
    //                        return Result<ExpendingToolResponseList>.Fail(request.NotFound);
    //                    }
    //                    DateTime projectStart = project.StartDate!.Value;
    //                    ExpendingToolResponseList result = new();


    //                    result.MapExpenses(project);
    //                    result.MapCapital(project);
    //                    result.MapEngineering(project);




    //                    return Result<ExpendingToolResponseList>.Success(result);
    //                });

    //        }


    //    }
    //    static ExpendingBudgetResponse MapExpendingBudget(this BudgetItem row)
    //    {
    //        return new()
    //        {
    //            Name = row.Name,
    //            Nomenclatore = row.Nomenclatore,
    //            Order = row.OrderList,
    //            BudgetUSD = row.BudgetUSD,
    //        };

    //    }
    //    static ExpendingBudgetResponse MapExpendingBudget(this BudgetItem row,
    //        List<ExpendingBudgetResponse> Capital, double percentage, double percentagetotal)
    //    {
    //        var total = Capital.Sum(x => x.BudgetUSD);
    //        var budget = 100 - percentagetotal == 0 ? 0 : total / (100 - percentagetotal) * percentage;
    //        return new()
    //        {
    //            Name = row.Name,
    //            Nomenclatore = row.Nomenclatore,
    //            Order = row.OrderList,
    //            BudgetUSD = Math.Round(budget),
    //        };

    //    }
    //    static ExpendingToolRow MapExpendingToolMonth(this BudgetItem row, int month, int year)
    //    {
    //        DateTime MonthNow = new DateTime(year, month, 1);
    //        double budget = row.NewGanttTasks.Where(x =>
    //                            x.EndDate.Month == month
    //                            && x.EndDate.Year == year).Sum(x => x.TotalBudgetAssigned);
    //        var monthname = MonthNow.ToString("MMM", new System.Globalization.CultureInfo("en-US"));
    //        string columnName = $"{monthname}-{year}";
    //        return new(1 + month, month, columnName, Math.Round(budget));

    //    }
    //    static ExpendingToolRow MapExpendingToolEnginering(this IEngineeringContingency row, int month, int year,
    //        List<ExpendingBudgetResponse> Capital, double percentagetotal)
    //    {
    //        double Percentage = (100 - percentagetotal) == 0 ? 0 : row.Percentage / (100 - percentagetotal);
    //        var budget = Capital.Sum(x => x.GetBudgetByMonth(month)) * Percentage;

    //        DateTime MonthNow = new DateTime(year, month, 1);

    //        var monthname = MonthNow.ToString("MMM", new System.Globalization.CultureInfo("en-US"));
    //        string columnName = $"{monthname}-{year}";
    //        return new(1 + month, month, columnName, Math.Round(budget));

    //    }
    //    static ExpendingToolRow MapExpendingToolEngineringNextYear(this IEngineeringContingency row, int year,
    //       List<ExpendingBudgetResponse> Capital, double percentagetotal)
    //    {
    //        int nexyyear = year + 1;
    //        double Percentage = (100 - percentagetotal) == 0 ? 0 : row.Percentage / (100 - percentagetotal);
    //        var budget = Capital.Sum(x => x.Columns.Where(c => c.Month == nexyyear).Sum(x => x.Budget)) * Percentage;


    //        string columnName = $"{nexyyear}+";
    //        return new(14, nexyyear, columnName, Math.Round(budget));

    //    }
    //    static ExpendingToolRow MapExpendingToolEngineringPreviousYear(this IEngineeringContingency row, int year,
    //      List<ExpendingBudgetResponse> Capital, double percentagetotal)
    //    {
    //        int nexyyear = year - 1;
    //        double Percentage = (100 - percentagetotal) == 0 ? 0 : row.Percentage / (100 - percentagetotal);
    //        var budget = Capital.Sum(x => x.Columns.Where(c => c.Month == nexyyear).Sum(x => x.Budget)) * Percentage;


    //        string columnName = $"{nexyyear}-";
    //        return new(14, nexyyear, columnName, Math.Round(budget));

    //    }

    //    static ExpendingToolRow MapExpendingToolNextYear(this BudgetItem row, int year)
    //    {
    //        int nexyyear = year + 1;

    //        double budget = row.NewGanttTasks.Where(x =>
    //                             x.EndDate.Year == nexyyear).Sum(x => x.TotalBudgetAssigned);

    //        string columnName = $"{nexyyear}+";
    //        return new(14, nexyyear, columnName, Math.Round(budget));

    //    }
    //    static ExpendingToolRow MapExpendingToolPreviousYear(this BudgetItem row, int year)
    //    {
    //        int nexyyear = year - 1;

    //        double budget = row.NewGanttTasks.Where(x =>
    //                             x.EndDate.Year == nexyyear).Sum(x => x.TotalBudgetAssigned);

    //        string columnName = $"{nexyyear}-";
    //        return new(1, nexyyear, columnName, Math.Round(budget));

    //    }
    //    static ExpendingToolResponseList MapExpenses(this ExpendingToolResponseList result, Project project)
    //    {
    //        int QueryYear = project.StartDate!.Value.Year;
    //        foreach (var budgetItem in project.Expenses)
    //        {
    //            var item = budgetItem.MapExpendingBudget();

    //            var rowPreviousYears = budgetItem.MapExpendingToolPreviousYear(QueryYear);

    //            item.Columns.Add(rowPreviousYears);


    //            for (int month = 1; month <= 12; month++)
    //            {
    //                ExpendingToolRow row = budgetItem.MapExpendingToolMonth(month, QueryYear);
    //                item.Columns.Add(row);
    //            }

    //            var rowNextYears = budgetItem.MapExpendingToolNextYear(QueryYear);

    //            item.Columns.Add(rowNextYears);

    //            result.ExpensesItems.Add(item);
    //        }
    //        return result;
    //    }
    //    static ExpendingToolResponseList MapCapital(this ExpendingToolResponseList result, Project project)
    //    {
    //        int QueryYear = project.StartDate!.Value.Year;
    //        foreach (var budgetItem in project.Capital)
    //        {
    //            var item = budgetItem.MapExpendingBudget();
    //            var rowPreviousYears = budgetItem.MapExpendingToolPreviousYear(QueryYear);

    //            item.Columns.Add(rowPreviousYears);

    //            for (int month = 1; month <= 12; month++)
    //            {
    //                ExpendingToolRow row = budgetItem.MapExpendingToolMonth(month, QueryYear);
    //                item.Columns.Add(row);
    //            }

    //            var rowOtherYears = budgetItem.MapExpendingToolNextYear(QueryYear);

    //            item.Columns.Add(rowOtherYears);

    //            result.CapitalItems.Add(item);
    //        }
    //        return result;
    //    }
    //    static ExpendingToolResponseList MapEngineering(this ExpendingToolResponseList result, Project project)
    //    {
    //        int QueryYear = project.StartDate!.Value.Year;
    //        var percentagecontingency = project.EngineeringContingencys.OfType<IEngineeringContingency>().Sum(x => x.Percentage);
    //        foreach (var budgetItem in project.EngineeringContingencys)
    //        {
    //            var percentage = ((IEngineeringContingency)budgetItem).Percentage;
    //            var item = budgetItem.MapExpendingBudget(result.CapitalItems, percentage, percentagecontingency);

    //            var rowPreviousYears = ((IEngineeringContingency)budgetItem)
    //            .MapExpendingToolEngineringPreviousYear(QueryYear, result.CapitalItems, percentagecontingency);
    //            item.Columns.Add(rowPreviousYears);
    //            result.EngineeringItems.Add(item);
    //            for (int month = 1; month <= 12; month++)
    //            {
    //                ExpendingToolRow rowEngineering =
    //                ((IEngineeringContingency)budgetItem)
    //                .MapExpendingToolEnginering(month, QueryYear, result.CapitalItems, percentagecontingency);
    //                item.Columns.Add(rowEngineering);
    //            }
    //            var rowOtherYears = ((IEngineeringContingency)budgetItem)
    //            .MapExpendingToolEngineringNextYear(QueryYear, result.CapitalItems, percentagecontingency);
    //            item.Columns.Add(rowOtherYears);
    //            result.EngineeringItems.Add(item);
    //        }
    //        return result;
    //    }
    //}
}