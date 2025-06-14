using Server.Database.Entities.ProjectManagements;

namespace Server.EndPoint.Assumptions.Queries
{
    public static class GetAssumptionByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Assumptions.EndPoint.GetById, async (GetAssumptionByIdRequest request, IQueryRepository Repository) =>
                {


                    Expression<Func<Assumption, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Assumptions.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map();
                    return Result.Success(response);

                });
            }
        }

        public static AssumptionResponse Map(this Assumption row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                Order = row.Order,
                
                
                ProjectId = row.ProjectId,
            };
        }

    }
}
