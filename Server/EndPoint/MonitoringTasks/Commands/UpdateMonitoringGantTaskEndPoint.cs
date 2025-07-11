using Shared.Models.MonitoringTask.Request;

namespace Server.EndPoint.MonitoringTasks.Commands
{

    public static class UpdateMonitoringGantTaskEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MonitoringTasks.EndPoint.CreateUpdate, async (UpdateMonitoringGanttTaskRequest Data, IRepository Repository) =>
                {
                    if (Data.GanttTaskId == Guid.Empty)
                        return Result.Fail(Data.NotFound);
                    Func<IQueryable<NewGanttTask>, IIncludableQueryable<NewGanttTask, object>> includes = x => x
                     .Include(x => x.Deliverable)
                     ;
                    Expression<Func<NewGanttTask, bool>> criteria = x => x.Id == Data.GanttTaskId;
                    var row = await Repository.GetAsync(Includes: includes, Criteria: criteria);

                    if (row == null)
                        return Result.Fail(Data.NotFound);

                    row.RealDurationInDays = Data.RealDurationInDays;
                    row.RealDurationInUnit = Data.RealDurationInUnit;
                    row.RealDurationUnit = Data.RealDurationUnit;
                    row.RealStartDate = Data.RealStartDate;
                    row.RealEndDate = Data.RealEndDate;
                    row.TaskStatus = Data.TaskStatus.Id;
                    await Repository.UpdateAsync(row);


                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));


                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
            private string[] GetCacheKeys(NewGanttTask row)
            {
                List<string> cacheKeys = [
                    .. StaticClass.DeliverableGanttTasks.Cache.Key(row.ProjectId)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
