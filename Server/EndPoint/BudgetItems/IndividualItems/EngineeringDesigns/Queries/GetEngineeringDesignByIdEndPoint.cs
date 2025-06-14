using Server.Database.Entities.BudgetItems.Commons;
using Server.EndPoint.MonitoringTasks.Queries;
using Server.EndPoint.Projects.Queries;
using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Models.BudgetItems.IndividualItems.EngineeringDesigns.Records;
using Shared.Models.BudgetItems.IndividualItems.EngineeringDesigns.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.EngineeringDesigns.Queries
{
    public static class GetEngineeringDesignByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.EngineeringDesigns.EndPoint.GetById, async (GetEngineeringDesignByIdRequest request, IQueryRepository Repository) =>
                {
                  

                    Expression<Func<EngineeringDesign, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.EngineeringDesigns.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map();
                    return Result.Success(response);

                });
            }
        }
        public static EngineeringDesignResponse Map(this EngineeringDesign row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                OrderList = row.OrderList,
                ProjectId = row.ProjectId,
                Nomenclatore = row.Nomenclatore,
                BudgetUSD = row.BudgetUSD,
                ActualUSD = row.ActualUSD,
                CommitmentUSD = row.CommitmentUSD,
                PotentialUSD = row.PotentialUSD,
                PurchaseOrders = row.PurchaseOrderItems == null ? new() : row.PurchaseOrderItems.Select(x => x.PurchaseOrder).Select(x => x.Map()).ToList(),
                BudgetItemGanttTasks = row.BudgetItemNewGanttTasks == null ? new() : row.BudgetItemNewGanttTasks.Select(x => x.Map()).ToList(),
            };
        }


    }
}
