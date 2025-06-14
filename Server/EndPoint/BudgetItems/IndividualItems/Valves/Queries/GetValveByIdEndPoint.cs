using Server.EndPoint.Brands.Queries;
using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Enums.ValvesEnum;
using Shared.Models.BudgetItems.IndividualItems.Valves.Records;
using Shared.Models.BudgetItems.IndividualItems.Valves.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.Valves.Queries
{
    public static class GetValveByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Valves.EndPoint.GetById, async (GetValveByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Valve>, IIncludableQueryable<Valve, object>> Includes = x => x

                    .Include(x => x.ValveItems)
                    .ThenInclude(x => x.ValveTemplate!).ThenInclude(x => x.BrandTemplate!)
                    ;

                    Expression<Func<Valve, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Valves.Cache.GetById(request.Id);
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
