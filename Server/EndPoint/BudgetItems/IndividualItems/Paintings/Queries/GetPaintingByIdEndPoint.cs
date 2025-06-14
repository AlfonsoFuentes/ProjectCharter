using Server.EndPoint.MonitoringTasks.Queries;
using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Models.BudgetItems.IndividualItems.Paintings.Records;
using Shared.Models.BudgetItems.IndividualItems.Paintings.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.Paintings.Queries
{
    public static class GetPaintingByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Paintings.EndPoint.GetById, async (GetPaintingByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Painting>, IIncludableQueryable<Painting, object>> Includes = x => null!;

                    ;

                    Expression<Func<Painting, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Paintings.Cache.GetById(request.Id);
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
        public static PaintingResponse Map(this Painting row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                OrderList = row.OrderList,
                ProjectId = row.ProjectId,
                Nomenclatore = row.Nomenclatore,
                UnitaryCost = row.UnitaryCost,
                Quantity = row.Quantity,
                ActualUSD = row.ActualUSD,
                CommitmentUSD = row.CommitmentUSD,
                PotentialUSD = row.PotentialUSD,
                BudgetUSD = row.BudgetUSD,
                PurchaseOrders = row.PurchaseOrderItems == null ? new() : row.PurchaseOrderItems.Select(x => x.PurchaseOrder).Select(x => x.Map()).ToList(),
                BudgetItemGanttTasks = row.BudgetItemNewGanttTasks == null ? new() : row.BudgetItemNewGanttTasks.Select(x => x.Map()).ToList(),
            };
        }


    }
}
