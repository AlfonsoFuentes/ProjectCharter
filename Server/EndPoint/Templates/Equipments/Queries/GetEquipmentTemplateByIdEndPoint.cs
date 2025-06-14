using Server.EndPoint.Brands.Queries;
using Server.ExtensionsMethods.EquipmentTemplateMapper;
using Shared.Models.Templates.Equipments.Records;

namespace Server.EndPoint.Templates.Equipments.Queries
{
    public static class GetEquipmentTemplateByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.EquipmentTemplates.EndPoint.GetById,
                    async (GetEquipmentTemplateByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<EquipmentTemplate>, IIncludableQueryable<EquipmentTemplate, object>> Includes = x =>x
                    .Include(x => x.NozzleTemplates)
                    .Include(x => x.BrandTemplate!);
                    ;

                    Expression<Func<EquipmentTemplate, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.EquipmentTemplates.Cache.GetById(request.Id);
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
