using Server.Database.Entities.ProjectManagements;
using Shared.Models.Constrainsts.Requests;

namespace Server.EndPoint.Constrainsts.Commands
{
    public static class DeleteConstrainstEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Constrainsts.EndPoint.Delete, async (DeleteConstrainstRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<Constrainst>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }

                    var cachekeys = GetCacheKeys(row);
                    
                    await Repository.RemoveAsync(row);

                  
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cachekeys);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }

            private string[] GetCacheKeys(Constrainst row)
            {
                List<string> cacheKeys = [
                  ..StaticClass.Constrainsts.Cache.Key(row.Id, row.ProjectId),

                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
