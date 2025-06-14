namespace Server.EndPoint.GanttTasks.Validators
{
    //public static class ValidateGanttTasksNameEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.GanttTasks.EndPoint.Validate, async (ValidateGanttTaskRequest Data, IQueryRepository Repository) =>
    //            {
    //                Expression<Func<GanttTask, bool>> CriteriaId = x => x.DeliverableId == Data.DeliverableId;

    //                Func<GanttTask, bool> CriteriaExist = x => Data.Id == null ?

    //                x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
    //                string CacheKey = StaticClass.GanttTasks.Cache.GetAll(Data.DeliverableId);

    //                return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
    //            });


    //        }
    //    }



    //}

}
