using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Server.EndPoint.Brands.Queries;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Records;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicValves.Queries
{
    public static class GetBasicValveByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicValves.EndPoint.GetById, async (GetBasicValveByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<BasicValveItem>, IIncludableQueryable<BasicValveItem, object>> Includes = x => x
                    .Include(x => x.Nozzles)
                    .Include(x => x.ValveTemplate!).ThenInclude(x => x.BrandTemplate!)
                    ;

                    Expression<Func<BasicValveItem, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.BasicValves.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria, Includes: Includes);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map();
                    return Result.Success(response);

                });
            }
        }



    }
}
