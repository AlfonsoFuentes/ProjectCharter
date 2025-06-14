using Server.Database.Entities.ProjectManagements;

namespace Server.EndPoint.Constrainsts.Queries
{
    public static class GetConstrainstByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Constrainsts.EndPoint.GetById, async (GetConstrainstByIdRequest request, IQueryRepository Repository) =>
                {
                   
                    Expression<Func<Constrainst, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Constrainsts.Cache.GetById(request.Id);
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

        public static ConstrainstResponse Map(this Constrainst row)
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
