using Shared.Models.Scopes.Requests;

namespace Server.EndPoint.Scopes.Commands
{
    public static class DeleteScopeEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Scopes.EndPoint.Delete, async (DeleteScopeRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<Scope>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }

                    var cache = GetCacheKeys(row);
                    
                    await Repository.RemoveAsync(row);
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
            private string[] GetCacheKeys(Scope row)
            {
                List<string> cacheKeys = [
                   
                    .. StaticClass.Scopes.Cache.Key(row.Id, row.ProjectId),
                 

                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
