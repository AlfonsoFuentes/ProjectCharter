namespace Server.EndPoint.GanttTasks.Commands
{
    //public static class DeleteGanttTaskEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.GanttTasks.EndPoint.Delete, async (DeleteGanttTaskRequest Data, IRepository Repository) =>
    //            {
    //                Func<IQueryable<GanttTask>, IIncludableQueryable<GanttTask, object>> Includes = x => null!;


    //                Expression<Func<GanttTask, bool>> Criteria = x => x.Id == Data.Id;


    //                var row = await Repository.GetAsync(Criteria: Criteria);


    //                if (row == null) { return Result.Fail(Data.NotFound); }

    //                await Repository.RemoveAsync(row);



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
