using Server.Database.Entities.ProjectManagements;
using Shared.Models.Scopes.Records;

namespace Server.EndPoint.Scopes.Queries
{
    public static class GetScopeByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Scopes.EndPoint.GetById, async (GetScopeByIdRequest request, IQueryRepository Repository) =>
                {

                    Func<IQueryable<Scope>, IIncludableQueryable<Scope, object>> Includes = x => null!;
            

                    Expression<Func<Scope, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Scopes.Cache.GetById(request.Id);
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
        public static ScopeResponse Map(this Scope row)
        {
            ScopeResponse result = new()
            {
                Id = row.Id,
                Name = row.Name,
              
                ProjectId = row.ProjectId,
                
                

                Order = row.Order,
            };
           
            return result;
        }


    }
}
