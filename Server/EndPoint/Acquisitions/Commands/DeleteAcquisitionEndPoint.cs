using Server.Database.Entities.ProjectManagements;
using Shared.Models.Acquisitions.Requests;

namespace Server.EndPoint.Acquisitions.Commands
{
    public static class DeleteAcquisitionEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Acquisitions.EndPoint.Delete, async (DeleteAcquisitionRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<Acquisition>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }


                    List<string> cache = [.. StaticClass.Acquisitions.Cache.Key(row.Id, row.ProjectId)];

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
