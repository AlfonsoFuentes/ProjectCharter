using Server.Database.Entities.BudgetItems.Commons;
using Server.EndPoint.MonitoringTasks.Queries;
using Server.EndPoint.Projects.Queries;
using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Models.BudgetItems.IndividualItems.Testings.Records;
using Shared.Models.BudgetItems.IndividualItems.Testings.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.Testings.Queries
{
    public static class GetTestingByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Testings.EndPoint.GetById, async (GetTestingByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Testing>, IIncludableQueryable<Testing, object>> Includes = x => null!;

                    ;

                    Expression<Func<Testing, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Testings.Cache.GetById(request.Id);
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
        public static TestingResponse Map(this Testing row)
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
