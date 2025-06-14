using Shared.Models.Resources.Requests;

namespace Server.EndPoint.Resources.Commands
{
    public static class DeleteResourceEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Resources.EndPoint.Delete, async (DeleteResourceRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<Resource>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }


                    List<string> cache = [.. StaticClass.Resources.Cache.Key(row.Id, row.ProjectId)];

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
