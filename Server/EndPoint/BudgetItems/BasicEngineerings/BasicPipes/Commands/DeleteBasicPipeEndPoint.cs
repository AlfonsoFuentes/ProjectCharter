using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Requests;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicPipes.Commands
{
    public static class DeleteBasicPipeEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicPipes.EndPoint.Delete, async (DeleteBasicPipeRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<BasicPipeItem>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    var cache = StaticClass.BudgetItems.Cache.GetAll(Data.ProjectId);

                    await Repository.RemoveAsync(row);

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });

            }
            private string[] GetCacheKeys(BasicPipeItem row)
            {

                List<string> cacheKeys = [
                 ..StaticClass.BudgetItems.Cache.Key(row.PipeId!.Value, row.ProjectId),

                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
