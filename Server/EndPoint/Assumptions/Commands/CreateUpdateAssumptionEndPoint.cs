using Server.Database.Entities.ProjectManagements;
using Shared.Models.Assumptions.Requests;
using System.Threading;

namespace Server.EndPoint.Assumptions.Commands
{

    public static class CreateUpdateAssumptionEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Assumptions.EndPoint.CreateUpdate, async (AssumptionResponse Data, IRepository Repository) =>
                {

                    Assumption? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<Assumption, Project>(Data.ProjectId);

                        row = Assumption.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<Assumption>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }

                    Data.Map(row);


                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));


                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
            private string[] GetCacheKeys(Assumption row)
            {
                List<string> cacheKeys = [
                    .. StaticClass.Assumptions.Cache.Key(row.Id, row.ProjectId)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }


        static Assumption Map(this AssumptionResponse request, Assumption row)
        {
            row.Name = request.Name;
            return row;
        }

    }

}
