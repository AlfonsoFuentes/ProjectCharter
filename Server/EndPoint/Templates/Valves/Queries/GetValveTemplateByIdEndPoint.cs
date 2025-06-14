

using Server.EndPoint.Brands.Queries;
using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Enums.NozzleTypes;
using Shared.Models.Templates.Valves.Records;
using Shared.Models.Templates.Valves.Responses;
using Shared.Models.Templates.NozzleTemplates;
using Shared.Enums.ValvesEnum;
using Server.EndPoint.Templates.Valves.Queries;

namespace Server.EndPoint.Templates.Valves.Queries
{
    public static class GetValveTemplateByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ValveTemplates.EndPoint.GetById,
                    async (GetValveTemplateByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<ValveTemplate>, IIncludableQueryable<ValveTemplate, object>> Includes = x =>x
                    .Include(x => x.NozzleTemplates)
                    .Include(x => x.BrandTemplate!);
                    ;

                    Expression<Func<ValveTemplate, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.ValveTemplates.Cache.GetById(request.Id);
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
