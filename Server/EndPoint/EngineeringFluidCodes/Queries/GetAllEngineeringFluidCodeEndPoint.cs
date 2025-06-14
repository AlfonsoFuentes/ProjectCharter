using Shared.Models.EngineeringFluidCodes.Records;
using Shared.Models.EngineeringFluidCodes.Responses;

namespace Server.EndPoint.EngineeringFluidCodes.Queries
{
    public static class GetAllEngineeringFluidCodeEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.EngineeringFluidCodes.EndPoint.GetAll, async (EngineeringFluidCodeGetAll request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<EngineeringFluidCode>, IIncludableQueryable<EngineeringFluidCode, object>>
                     Includes = null!;

                    string CacheKey = StaticClass.EngineeringFluidCodes.Cache.GetAll;
                    var rows = await Repository.GetAllAsync(CacheKey, Includes: Includes);

                    if (rows == null)
                    {
                        return Result<EngineeringFluidCodeResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.EngineeringFluidCodes.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    EngineeringFluidCodeResponseList response = new EngineeringFluidCodeResponseList()
                    {
                        Items = maps
                    };
                    return Result<EngineeringFluidCodeResponseList>.Success(response);

                });
            }
        }
    }
}