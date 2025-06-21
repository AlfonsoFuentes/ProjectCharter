using Server.Database.Entities.ProjectManagements;
using Shared.Models.OtherTasks.Requests;

namespace Server.EndPoint.OtherTasks.Commands
{
    public static class DeleteOtherTaskEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.OtherTasks.EndPoint.Delete, async (DeleteOtherTaskRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<OtherTask>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }


                    List<string> cache = [.. StaticClass.OtherTasks.Cache.Key(row.Id, row.ProjectId)];

                    await Repository.RemoveAsync(row);
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }

}
