using Shared.Models.OtherTasks.Requests;

namespace Server.EndPoint.OtherTasks.Commands
{
    public static class DeleteGroupOtherTaskEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.OtherTasks.EndPoint.DeleteGroup, async (DeleteGroupOtherTaskRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<OtherTask>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.OtherTasks.Cache.GetAll(Data.ProjectId);

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }

}
