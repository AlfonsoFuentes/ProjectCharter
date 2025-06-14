using Server.ExtensionsMethods.InstrumentTemplateMapper;
using Shared.Models.Templates.Instruments.Records;
using Shared.Models.Templates.Instruments.Responses;

namespace Server.EndPoint.Templates.Instruments.Queries
{
    public static class GetAllInstrumentTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InstrumentTemplates.EndPoint.GetAll, async (InstrumentTemplateGetAll request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<InstrumentTemplate>, IIncludableQueryable<InstrumentTemplate, object>> Includes = x => x
                    .Include(x => x.NozzleTemplates)
                    .Include(x => x.BrandTemplate!)
                     ;

                    string CacheKey = StaticClass.InstrumentTemplates.Cache.GetAll;
                    var rows = await Repository.GetAllAsync(CacheKey, Includes: Includes);

                    if (rows == null)
                    {
                        return Result<InstrumentTemplateResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.InstrumentTemplates.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    InstrumentTemplateResponseList response = new InstrumentTemplateResponseList()
                    {
                        Items = maps
                    };
                    return Result<InstrumentTemplateResponseList>.Success(response);

                });
            }
        }
    }
}