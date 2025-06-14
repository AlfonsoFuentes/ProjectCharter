

using Shared.Models.EngineeringFluidCodes.Records;
using Shared.Models.EngineeringFluidCodes.Responses;

namespace Server.EndPoint.EngineeringFluidCodes.Queries
{
    public static class GetEngineeringFluidCodeByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.EngineeringFluidCodes.EndPoint.GetById,
                    async (GetEngineeringFluidCodeByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<EngineeringFluidCode>, IIncludableQueryable<EngineeringFluidCode, object>> Includes = null!
                    ;

                    Expression<Func<EngineeringFluidCode, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.EngineeringFluidCodes.Cache.GetById(request.Id);
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


        public static EngineeringFluidCodeResponse Map(this EngineeringFluidCode row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,

                Code = row.Code,
            };
        }



    }
}
