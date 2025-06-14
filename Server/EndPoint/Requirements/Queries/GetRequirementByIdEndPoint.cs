using Server.Database.Entities.ProjectManagements;

namespace Server.EndPoint.Requirements.Queries
{
    public static class GetRequirementByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Requirements.EndPoint.GetById, async (GetRequirementByIdRequest request, IQueryRepository Repository) =>
                {
                    

                    Expression<Func<Requirement, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Requirements.Cache.GetById(request.Id);
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

        public static RequirementResponse Map(this Requirement row)
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
