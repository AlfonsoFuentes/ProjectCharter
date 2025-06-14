using Server.Database.Entities.ProjectManagements;
using Shared.Models.Objectives.Requests;

namespace Server.EndPoint.Objectives.Commands
{
    public static class DeleteObjectiveEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Objectives.EndPoint.Delete, async (DeleteObjectiveRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<Objective>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }

                    var cachekeys = GetCacheKeys(row);
                    await Repository.RemoveAsync(row);


                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cachekeys);

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });
            }
            private string[] GetCacheKeys(Objective row)
            {
                List<string> cacheKeys = [
                    .. StaticClass.Objectives.Cache.Key(row.Id, row.ProjectId),
             

                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
