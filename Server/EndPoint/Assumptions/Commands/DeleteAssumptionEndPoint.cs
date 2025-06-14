using Server.Database.Entities.ProjectManagements;
using Shared.Models.Assumptions.Requests;

namespace Server.EndPoint.Assumptions.Commands
{
    public static class DeleteAssumptionEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Assumptions.EndPoint.Delete, async (DeleteAssumptionRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<Assumption>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    var cache = GetCacheKeys(row);

                    await Repository.RemoveAsync(row);
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
            private string[] GetCacheKeys(Assumption row)
            {
                List<string> cacheKeys = [   
                    .. StaticClass.Assumptions.Cache.Key(row.Id, row.ProjectId)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
