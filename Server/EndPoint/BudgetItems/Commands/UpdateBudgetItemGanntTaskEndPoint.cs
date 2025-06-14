using Shared.Models.BudgetItems.Requests;

namespace Server.EndPoint.BudgetItems.Commands
{
    //public static class UpdateBudgetItemGanntTaskEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.BudgetItems.EndPoint.UpdateGanttTask, async (UpdateBudgetItemGanntTaskRequest Data, IRepository Repository) =>
    //            {
    //                var row = await Repository.GetByIdAsync<BudgetItem>(Data.Id);
    //                if (row == null) { return Result.Fail(Data.NotFound); }


    //                //row.GanttTaskId = Data.GanttTaskId;
                   
    //                await Repository.UpdateAsync(row);

    //                var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));
    //                return Result.EndPointResult(result,
    //                    Data.Succesfully,
    //                    Data.Fail);

    //            });
    //        }
    //        private string[] GetCacheKeys(BudgetItem row)
    //        {
    //           // var deliverable = row.GanttTaskId.HasValue ? StaticClass.GanttTasks.Cache.Key(row.GanttTaskId!.Value, row.ProjectId) : new[] { string.Empty };
    //            var budgetitems = StaticClass.BudgetItems.Cache.Key(row.Id, row.ProjectId/*, row.GanttTaskId*/);
                

    //            List<string> cacheKeys = [
    //                 ..budgetitems,
    //                 ..deliverable,

    //            ];

    //            return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
    //        }
    //    }




    //}

}
