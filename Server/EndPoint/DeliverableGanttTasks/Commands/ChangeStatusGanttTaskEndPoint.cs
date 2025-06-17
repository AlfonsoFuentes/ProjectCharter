using Shared.Models.DeliverableGanttTasks.Records;

namespace Server.EndPoint.DeliverableGanttTasks.Commands
{
    public static class ChangeStatusGanttTaskEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.DeliverableGanttTasks.EndPoint.UpdateStatus, async (ChangeStatusGanttTaskRequest Data, IRepository Repository) =>
                {
                    var ganttTask = await Repository.GetByIdAsync<NewGanttTask>(Data.Id);
                    if (ganttTask == null)
                    {
                        return Result.Fail(Data.NotFound);
                    }

                    await Repository.UpdateAsync(ganttTask);

                    ganttTask.TaskStatus = Data.Status.Id;
                    var cacheDeliverable = StaticClass.DeliverableGanttTasks.Cache.Key(Data.ProjectId);
                    string cacheBudget = StaticClass.BudgetItems.Cache.GetAll(Data.ProjectId);
                    List<string> cache = [.. cacheDeliverable, cacheBudget];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                      Data.Succesfully,
                      Data.Fail);
                });
            }
        }
    }
}
