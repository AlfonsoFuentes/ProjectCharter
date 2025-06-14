using Shared.Models.Templates.Valves.Records;
using Shared.Models.Templates.Valves.Responses;

namespace Server.EndPoint.Templates.Valves.Queries
{
    public static class GetAllValveTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ValveTemplates.EndPoint.GetAll, async (ValveTemplateGetAll request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<ValveTemplate>, IIncludableQueryable<ValveTemplate, object>> Includes = x => x
                    .Include(x => x.NozzleTemplates)
                    .Include(x => x.BrandTemplate!)
                     ;

                    string CacheKey = StaticClass.ValveTemplates.Cache.GetAll;
                    var rows = await Repository.GetAllAsync(CacheKey, Includes: Includes);

                    if (rows == null)
                    {
                        return Result<ValveTemplateResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.ValveTemplates.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    ValveTemplateResponseList response = new ValveTemplateResponseList()
                    {
                        Items = maps
                    };
                    return Result<ValveTemplateResponseList>.Success(response);

                });
            }
        }
    }
}