using Server.Database.Entities.ProjectManagements;
using Shared.Models.Communications.Requests;

namespace Server.EndPoint.Communications.Commands
{
    public static class DeleteCommunicationEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Communications.EndPoint.Delete, async (DeleteCommunicationRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<Communication>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }


                    List<string> cache = [.. StaticClass.Communications.Cache.Key(row.Id, row.ProjectId)];

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
