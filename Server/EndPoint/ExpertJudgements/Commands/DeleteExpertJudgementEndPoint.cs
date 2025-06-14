using Server.Database.Entities.ProjectManagements;
using Shared.Models.ExpertJudgements.Requests;

namespace Server.EndPoint.ExpertJudgements.Commands
{
    public static class DeleteExpertJudgementEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ExpertJudgements.EndPoint.Delete, async (DeleteExpertJudgementRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<ExpertJudgement>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }


                    var cache = GetCacheKeys(row);

                    await Repository.RemoveAsync(row);
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
            private string[] GetCacheKeys(ExpertJudgement row)
            {
                List<string> cacheKeys = [
                    StaticClass.ExpertJudgements.Cache.GetAll(row.ProjectId)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
