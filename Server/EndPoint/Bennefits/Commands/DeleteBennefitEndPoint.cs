using Server.Database.Entities.ProjectManagements;
using Shared.Models.Bennefits.Requests;

namespace Server.EndPoint.Bennefits.Commands
{
    public static class DeleteBennefitEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Bennefits.EndPoint.Delete, async (DeleteBennefitRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<Bennefit>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }


                    List<string> cache = [
                    ..StaticClass.Bennefits.Cache.Key(row.Id, row.ProjectId)];

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
