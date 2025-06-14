using Server.Database.Entities.ProjectManagements;
using Shared.Models.Requirements.Requests;

namespace Server.EndPoint.Requirements.Commands
{
    public static class DeleteRequirementEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Requirements.EndPoint.Delete, async (DeleteRequirementRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<Requirement>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }

                    var cache = GetCacheKeys(row);

                    await Repository.RemoveAsync(row);
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
            private string[] GetCacheKeys(Requirement row)
            {
                List<string> cacheKeys = [
                ..StaticClass.Requirements.Cache.Key(row.Id, row.ProjectId),
     

                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
