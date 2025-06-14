using Shared.Models.Backgrounds.Requests;

namespace Server.EndPoint.BackGrounds.Commands
{
    public static class DeleteBackGroundEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BackGrounds.EndPoint.Delete, async (DeleteBackGroundRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<BackGround>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [StaticClass.BackGrounds.Cache.GetAll(row.ProjectId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());


                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }

}
