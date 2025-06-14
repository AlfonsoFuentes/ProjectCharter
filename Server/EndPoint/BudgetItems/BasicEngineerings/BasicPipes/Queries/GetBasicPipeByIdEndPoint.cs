using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Server.EndPoint.Brands.Queries;
using Server.EndPoint.EngineeringFluidCodes.Queries;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Records;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicPipes.Queries
{
    public static class GetBasicPipeByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicPipes.EndPoint.GetById, async (GetBasicPipeByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<BasicPipeItem>, IIncludableQueryable<BasicPipeItem, object>> Includes = x => x
           
                    .Include(x => x.FluidCode)
                    .Include(x => x.PipeTemplate!).ThenInclude(x=>x.BrandTemplate!);
            
                    Expression<Func<BasicPipeItem, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.BasicPipes.Cache.GetById(request.Id);
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
