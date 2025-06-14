using Server.Database.Entities.ProjectManagements;
using Shared.Models.Resources.Records;
using Shared.Models.Resources.Responses;

namespace Server.EndPoint.Resources.Queries
{
    public static class GetResourceByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Resources.EndPoint.GetById, async (GetResourceByIdRequest request, IQueryRepository Repository) =>
                {
                   

                    Expression<Func<Resource, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Resources.Cache.GetById(request.Id);
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

        public static ResourceResponse Map(this Resource row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                ProjectId = row.ProjectId,
                Order=row.Order,
                
                

            };
        }

    }
}
