using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Server.EndPoint.Brands.Queries;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Records;
namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicEquipments.Queries
{
    public static class GetBasicEquipmentByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicEquipments.EndPoint.GetById, async (GetBasicEquipmentByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<BasicEquipmentItem>, IIncludableQueryable<BasicEquipmentItem, object>> Includes = x => x
                    .Include(x => x.Nozzles)
                    .Include(x => x.EquipmentTemplate!).ThenInclude(x => x.BrandTemplate!)
                   ;
                    Expression<Func<BasicEquipmentItem, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.BasicEquipments.Cache.GetById(request.Id);
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
