using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Server.Database.Entities.ProjectManagements;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Requests;
using Shared.Models.BudgetItems.IndividualItems.Valves.Requests;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicValves.Commands
{
    public static class DeleteBasicValveEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicValves.EndPoint.Delete, async (DeleteBasicValveRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<BasicValveItem>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    var cache = StaticClass.BudgetItems.Cache.GetAll(Data.ProjectId);

                    await Repository.RemoveAsync(row);
                
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });

            }
            private string[] GetCacheKeys(BasicValveItem row)
            {
              
                List<string> cacheKeys = [
                 ..StaticClass.BudgetItems.Cache.Key(row.ValveId!.Value, row.ProjectId/*, row.GanttTaskId*/),
        
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
