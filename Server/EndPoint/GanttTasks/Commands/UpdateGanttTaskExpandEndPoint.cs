namespace Server.EndPoint.GanttTasks.Commands
{
    //public static class UpdateGanttTaskExpandEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.GanttTasks.EndPoint.UpdateExpand, async (UpdateGanttTaskExpandRequest Data, IRepository Repository) =>
    //            {
    //                var row = await Repository.GetByIdAsync<GanttTask>(Data.Id);

    //                if (row == null)
    //                    return Result.Fail();

    //                await Repository.UpdateAsync(row);

    //                row.IsExpanded = Data.IsExpanded;
    //                var cache = StaticClass.GanttTasks.Cache.GetAll(Data.ProjectId);
    //                var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);

    //                return Result.EndPointResult(result,
    //                    Data.Succesfully,
    //                    Data.Fail);


    //            });
    //        }

    //    }



    //}
}
