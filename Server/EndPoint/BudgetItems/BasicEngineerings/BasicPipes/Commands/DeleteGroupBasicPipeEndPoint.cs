using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Requests;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Requests;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicPipes.Commands
{
    public static class DeleteGroupBasicPipeEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicPipes.EndPoint.DeleteGroup, async (DeleteGroupBasicPipeRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<BasicPipeItem>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }
                    var cache = StaticClass.BudgetItems.Cache.GetAll(Data.ProjectId);

           

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }

}
