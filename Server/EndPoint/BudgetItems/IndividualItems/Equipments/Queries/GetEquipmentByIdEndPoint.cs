using Server.EndPoint.Brands.Queries;
using Shared.Enums.Materials;
using Shared.Models.BudgetItems.IndividualItems.Equipments.Records;
namespace Server.EndPoint.BudgetItems.IndividualItems.Equipments.Queries
{
    public static class GetEquipmentByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Equipments.EndPoint.GetById, async (GetEquipmentByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Equipment>, IIncludableQueryable<Equipment, object>> Includes = x => x
                    .Include(x => x.EquipmentItems)

                    .ThenInclude(x => x.EquipmentTemplate!).ThenInclude(x => x.BrandTemplate!)
                   ;
                    Expression<Func<Equipment, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Equipments.Cache.GetById(request.Id);
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
