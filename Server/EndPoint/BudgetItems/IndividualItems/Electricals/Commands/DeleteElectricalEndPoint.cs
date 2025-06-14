using Server.Database.Entities.BudgetItems.Commons;
using Server.Database.Entities.ProjectManagements;
using Shared.Models.BudgetItems.IndividualItems.Electricals.Requests;
using static Shared.StaticClasses.StaticClass;

namespace Server.EndPoint.BudgetItems.IndividualItems.Electricals.Commands
{
    public static class DeleteElectricalEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Electricals.EndPoint.Delete, async (DeleteElectricalRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<Electrical>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    var cachekeys = GetCacheKeys(row);

                    await Repository.RemoveAsync(row);
                    //if (Data.GanttTaskId.HasValue)
                    //{
                    //    var deliverable = await Repository.GetByIdAsync<GanttTask>(Data.GanttTaskId.Value);
                    //    if (deliverable != null)
                    //    {
                    //        deliverable.ShowBudgetItems = true;
                    //        await Repository.UpdateAsync(deliverable);
                    //    }

                    //}
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cachekeys);

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });

            }
            private string[] GetCacheKeys(BudgetItem row)
            {
                //var deliverable = row.GanttTaskId.HasValue ? StaticClass.GanttTasks.Cache.Key(row.GanttTaskId!.Value, row.ProjectId) : new[] { string.Empty };
                List<string> cacheKeys = [
                 ..StaticClass.BudgetItems.Cache.Key(row.Id, row.ProjectId/*, row.GanttTaskId*/),
                 //..deliverable
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
