using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Server.EndPoint.Brands.Queries;
using Server.EndPoint.PurchaseOrders.Queries;
using Server.ExtensionsMethods.InstrumentTemplateMapper;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Records;
namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicInstruments.Queries
{
    public static class GetBasicInstrumentByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicInstruments.EndPoint.GetById, async (GetBasicInstrumentByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<BasicInstrumentItem>, IIncludableQueryable<BasicInstrumentItem, object>> Includes = x => x
                 .Include(x => x.Nozzles)
                    .Include(x => x.InstrumentTemplate!).ThenInclude(x => x.BrandTemplate!)
               ;
                    Expression<Func<BasicInstrumentItem, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.BasicInstruments.Cache.GetById(request.Id);
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
