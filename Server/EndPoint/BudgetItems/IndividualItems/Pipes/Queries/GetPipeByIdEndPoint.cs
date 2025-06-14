using Server.EndPoint.Brands.Queries;
using Server.EndPoint.EngineeringFluidCodes.Queries;
using Server.ExtensionsMethods.Pipings;
using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Records;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.Pipes.Queries
{
    public static class GetPipeByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Pipes.EndPoint.GetById, async (GetPipeByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Pipe>, IIncludableQueryable<Pipe, object>> Includes = x => x
                    .Include(x => x.PipeItems)
                    .ThenInclude(x => x.FluidCode)
                    .Include(x => x.PipeItems).ThenInclude(x => x.PipeTemplate!).ThenInclude(x=>x.BrandTemplate!);
            
                    Expression<Func<Pipe, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Pipes.Cache.GetById(request.Id);
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
