using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Shared.Models.Brands.Requests;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Requests;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicEquipments.Commands
{
    public static class DeleteBasicEquipmentEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicEquipments.EndPoint.Delete, async (DeleteBasicEquipmentRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<BasicEquipmentItem>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    var cache = StaticClass.BudgetItems.Cache.GetAll(Data.ProjectId);

                    await Repository.RemoveAsync(row);
                    
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });

            }
            private string[] GetCacheKeys(BasicEquipmentItem row)
            {
              
                List<string> cacheKeys = [
                 ..StaticClass.BudgetItems.Cache.Key(row.EquipmentId!.Value, row.ProjectId),
          
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
