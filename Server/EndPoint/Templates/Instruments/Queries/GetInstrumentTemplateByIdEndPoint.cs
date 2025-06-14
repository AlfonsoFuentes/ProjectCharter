using Server.EndPoint.Brands.Queries;
using Server.EndPoint.Templates.Instruments.Queries;
using Server.ExtensionsMethods.InstrumentTemplateMapper;
using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.Instruments;
using Shared.Enums.Materials;
using Shared.Enums.NozzleTypes;
using Shared.Enums.ValvesEnum;
using Shared.Models.Templates.Instruments.Records;
using Shared.Models.Templates.Instruments.Responses;
using Shared.Models.Templates.NozzleTemplates;

namespace Server.EndPoint.Templates.Instruments.Queries
{
    public static class GetInstrumentTemplateByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InstrumentTemplates.EndPoint.GetById,
                    async (GetInstrumentTemplateByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<InstrumentTemplate>, IIncludableQueryable<InstrumentTemplate, object>> Includes = x =>x
                    .Include(x => x.NozzleTemplates)
                    .Include(x => x.BrandTemplate!);
                    ;

                    Expression<Func<InstrumentTemplate, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.InstrumentTemplates.Cache.GetById(request.Id);
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
