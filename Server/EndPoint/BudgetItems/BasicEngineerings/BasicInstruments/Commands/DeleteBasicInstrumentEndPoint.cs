using Server.Database.Entities.BudgetItems.Commons;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Server.Database.Entities.ProjectManagements;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Requests;
using Shared.Models.BudgetItems.IndividualItems.Instruments.Requests;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicInstruments.Commands
{
    public static class DeleteBasicInstrumentEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicInstruments.EndPoint.Delete, async (DeleteBasicInstrumentRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<BasicInstrumentItem>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    var cache = StaticClass.BudgetItems.Cache.GetAll(Data.ProjectId);

                    await Repository.RemoveAsync(row);
                  
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });

            }
            private string[] GetCacheKeys(BasicInstrumentItem row)
            {
                
                List<string> cacheKeys = [
                 ..StaticClass.BudgetItems.Cache.Key(row.InstrumentId!.Value, row.ProjectId),
             
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
