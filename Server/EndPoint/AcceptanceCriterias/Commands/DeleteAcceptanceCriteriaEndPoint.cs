using Server.Database.Entities.ProjectManagements;
using Shared.Models.AcceptanceCriterias.Requests;

namespace Server.EndPoint.AcceptanceCriterias.Commands
{
    public static class DeleteAcceptanceCriteriaEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.AcceptanceCriterias.EndPoint.Delete, async (DeleteAcceptanceCriteriaRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<AcceptanceCriteria>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }


                    List<string> cache = [.. StaticClass.AcceptanceCriterias.Cache.Key(row.Id, row.ProjectId)];

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
