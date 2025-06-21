using Server.Database.Entities.ProjectManagements;
using Shared.Models.OtherTasks.Mappers;

namespace Server.EndPoint.OtherTasks.Commands
{
    public static class ChangeOtherTaskOrderUpEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.OtherTasks.EndPoint.UpdateUp, async (ChangeOtherTaskOrderUpRequest Data, IRepository Repository) =>
                {


                    var row = await Repository.GetByIdAsync<OtherTask>(Data.Id);

                    if (row == null) { return Result.Fail(Data.NotFound); }
                    if (row.Order == 1) { return Result.Success(Data.Succesfully); }

                    Expression<Func<OtherTask, bool>> Criteria = x => x.ProjectId == Data.ProjectId && x.Order == row.Order - 1;

                    var previousRow = await Repository.GetAsync(Criteria: Criteria);

                    if (previousRow == null) { return Result.Fail(Data.NotFound); }

                    await Repository.UpdateAsync(previousRow);
                    await Repository.UpdateAsync(row);

                    row.Order = row.Order - 1;
                    previousRow.Order = row.Order + 1;

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });
            }
            private string[] GetCacheKeys(OtherTask row)
            {
                List<string> cacheKeys = [
                             
                    .. StaticClass.OtherTasks.Cache.Key(row.Id,row.ProjectId)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }



    }

}
