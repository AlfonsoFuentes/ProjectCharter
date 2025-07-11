using Server.EndPoint.BudgetItems.IndividualItems.Contingencys.Queries;
using Server.EndPoint.BudgetItems.IndividualItems.Engineerings.Queries;
using Server.EndPoint.Communications.Queries;
using Shared.Enums.CostCenter;
using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;

namespace Server.EndPoint.BudgetItems.Queries
{
    public static class GetAllBudgetItemEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BudgetItems.EndPoint.GetAll, async (BudgetItemGetAll request, IQueryRepository repository) =>
                {
                    var row = await GetBudgetItemAsync(request, repository);

                    if (row == null)
                    {
                        return Result<BudgetItemResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.BudgetItems.ClassLegend));
                    }

                    BudgetItemResponseList response = new()
                    {
                        ProjectId = request.ProjectId,
                        Alterations = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Alteration>().Select(x => x.Map()).ToList(),
                        Structurals = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Structural>().Select(x => x.Map()).ToList(),
                        Foundations = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Foundation>().Select(x => x.Map()).ToList(),
                        Equipments = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Equipment>().Select(x => x.Map()).ToList(),

                        Valves = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Valve>().Select(x => x.Map()).ToList(),
                        Electricals = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Electrical>().Select(x => x.Map()).ToList(),
                        Pipings = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Pipe>().Select(x => x.Map()).ToList(),
                        Instruments = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Instrument>().Select(x => x.Map()).ToList(),

                        EHSs = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<EHS>().Select(x => x.Map()).ToList(),
                        Paintings = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Painting>().Select(x => x.Map()).ToList(),
                        Taxes = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Tax>().Select(x => x.Map()).ToList(),
                        Testings = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Testing>().Select(x => x.Map()).ToList(),

                        EngineeringDesigns = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<EngineeringDesign>().Select(x => x.Map()).ToList(),
                        Engineerings = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Engineering>().Select(x => x.Map()).ToList(),
                        Contingencys = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Contingency>().Select(x => x.Map()).ToList(),
                        IsProductive = row.IsProductiveAsset,

                        PercentageContingency = row.PercentageContingency,
                        PercentageEngineering = row.PercentageEngineering,
                        PercentageTaxes = row.PercentageTaxProductive,
                        CostCenter = CostCenterEnum.GetType(row.CostCenter),
                        ProjectNumber = $"CEC0000{row.ProjectNumber}",
                        Status = ProjectStatusEnum.GetType(row.Status),


                    };
                    double totalpercentage = response.PercentageContingency + response.PercentageEngineering;

                    if (100 - totalpercentage > 0)
                    {
                        var contingency = response.Contingencys.FirstOrDefault();
                        if (contingency != null)
                        {
                            contingency.BudgetUSD = Math.Round(response.TotalCapital /
                            (100 - totalpercentage) * contingency.Percentage, 1);
                        }
                        var engineering = response.Engineerings.FirstOrDefault();
                        if (engineering != null)
                        {
                            engineering.BudgetUSD = Math.Round(response.TotalCapital /
                           (100 - totalpercentage) * engineering.Percentage, 1);
                        }
                    }






                    return Result<BudgetItemResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetBudgetItemAsync(BudgetItemGetAll request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x

                .Include(p => p.BudgetItems).ThenInclude(x => (x as Instrument)!.InstrumentItems!).ThenInclude(x => x.InstrumentTemplate!).ThenInclude(x => x.BrandTemplate!)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Pipe)!.PipeItems!).ThenInclude(x => x.FluidCode)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Pipe)!.PipeItems!).ThenInclude(x => x.PipeTemplate!)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Valve)!.ValveItems!).ThenInclude(x => x.ValveTemplate!).ThenInclude(x => x.BrandTemplate!)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Equipment)!.EquipmentItems!).ThenInclude(x => x.EquipmentTemplate!).ThenInclude(x => x.BrandTemplate!)
                .Include(x => x.BudgetItems).ThenInclude(x => x.BudgetItemNewGanttTasks)/*.ThenInclude(x => x.SelectedBasicEngineeringItem!)*/

                ;

                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.BudgetItems.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }


        }
    }
}