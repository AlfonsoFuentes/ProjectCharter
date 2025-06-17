using Server.Database.Entities.BudgetItems.EngineeringContingency;
using Server.EndPoint.BudgetItems.IndividualItems.Contingencys.Queries;
using Server.EndPoint.BudgetItems.IndividualItems.Engineerings.Queries;
using Server.EndPoint.Communications.Queries;
using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Enums.CostCenter;
using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;
using static Shared.StaticClasses.StaticClass;

namespace Server.EndPoint.BudgetItems.Queries
{
    public static class GetAllBudgetItemWithPurchaseOrdersEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BudgetItems.EndPoint.GetAllWithPurchaseorder, async (BudgetItemWithPurchaseOrderGetAll request, IQueryRepository repository) =>
                {
                    var row = await GetBudgetItemAsync(request, repository);

                    if (row == null)
                    {
                        return Result<BudgetItemWithPurchaseOrderResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.BudgetItems.ClassLegend));
                    }

                    BudgetItemWithPurchaseOrderResponseList response = new()
                    {
                       


                    };
                    response.Alterations = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.Alterations.Select(x => x.Map()).ToList();
                    response.Structurals = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.Structurals.Select(x => x.Map()).ToList();
                    response.Foundations = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.Foundations.Select(x => x.Map()).ToList();
                    response.Equipments = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.Equipments.Select(x => x.Map()).ToList();

                    response.Valves = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.Valves.Select(x => x.Map()).ToList();
                    response.Electricals = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.Electricals.Select(x => x.Map()).ToList();
                    response.Pipings = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.Pipings.Select(x => x.Map()).ToList();
                    response.Instruments = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.Instruments.Select(x => x.Map()).ToList();

                    response.EHSs = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.EHSs.Select(x => x.Map()).ToList();
                    response.Paintings = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.Paintings.Select(x => x.Map()).ToList();
                    response.Taxes = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.Taxes.Select(x => x.Map()).ToList();
                    response.Testings = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.Testings.Select(x => x.Map()).ToList();
                    response.Salaries = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.EngineeringSalaries.Select(x => x.Map()).ToList();
                    response.Contingencys = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.Contingencys.Select(x => x.Map()).ToList();
                    response.EngineeringDesigns = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.EngineeringDesigns.Select(x => x.Map()).ToList();
                    response.IsProductiveAsset = row.IsProductiveAsset;
                    response.PercentageContingency = row.PercentageContingency;
                    response.PercentageEngineering = row.PercentageEngineering;
                    response.PercentageTaxes = row.PercentageTaxProductive;
                    response.CostCenter = CostCenterEnum.GetType(row.CostCenter);
                    response.ProjectId = row.Id;
                    response.ProjectNumber = $"CEC0000{row.ProjectNumber}";
    


                    return Result<BudgetItemWithPurchaseOrderResponseList>.Success(response);
                });
            }

            private static async Task<Project?> GetBudgetItemAsync(BudgetItemWithPurchaseOrderGetAll request, IQueryRepository repository)
            {

                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x
                .Include(p => p.BudgetItems).ThenInclude(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrder).ThenInclude(x => x.Supplier)
                .Include(p => p.BudgetItems).ThenInclude(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
               
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Instrument)!.InstrumentItems!).ThenInclude(x => x.InstrumentTemplate!).ThenInclude(x => x.BrandTemplate)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Pipe)!.PipeItems!).ThenInclude(x => x.FluidCode)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Pipe)!.PipeItems!).ThenInclude(x => x.PipeTemplate)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Valve)!.ValveItems!).ThenInclude(x => x.ValveTemplate!).ThenInclude(x => x.BrandTemplate)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Equipment)!.EquipmentItems!).ThenInclude(x=>x.EquipmentTemplate!).ThenInclude(x=>x.BrandTemplate!)

                ;
                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.BudgetItems.Cache.GetAllWithPurchaseOrder(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }


        }
    }
}