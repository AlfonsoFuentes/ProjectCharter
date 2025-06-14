using Server.ExtensionsMethods.EquipmentTemplateMapper;
using Shared.Models.Templates.Equipments.Records;
using Shared.Models.Templates.Equipments.Responses;

namespace Server.EndPoint.Templates.Equipments.Queries
{
    public static class GetAllEquipmentTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.EquipmentTemplates.EndPoint.GetAll, async (EquipmentTemplateGetAll request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<EquipmentTemplate>, IIncludableQueryable<EquipmentTemplate, object>> Includes = x => x
                    .Include(x => x.NozzleTemplates)
                    .Include(x => x.BrandTemplate!)
                     ;

                    string CacheKey = StaticClass.EquipmentTemplates.Cache.GetAll;
                    var rows = await Repository.GetAllAsync(CacheKey, Includes: Includes);

                    if (rows == null)
                    {
                        return Result<EquipmentTemplateResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.EquipmentTemplates.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    EquipmentTemplateResponseList response = new EquipmentTemplateResponseList()
                    {
                        Items = maps
                    };
                    return Result<EquipmentTemplateResponseList>.Success(response);

                });
            }
        }
    }
}