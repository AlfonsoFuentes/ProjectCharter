using Server.Database.Entities.ProjectManagements;
using Shared.Models.KnownRisks.Requests;

namespace Server.EndPoint.KnownRisks.Commands
{
    public static class DeleteKnownRiskEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.KnownRisks.EndPoint.Delete, async (DeleteKnownRiskRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<KnownRisk>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }


                    var cache = GetCacheKeys(row);

                    await Repository.RemoveAsync(row);

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
            private string[] GetCacheKeys(KnownRisk row)
            {
                List<string> cacheKeys = [
                
                    StaticClass.KnownRisks.Cache.GetAll(row.ProjectId),
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
