using Shared.Models.Objectives.Responses;

namespace Server.EndPoint.Objectives.Commands
{

    public static class CreateUpdateObjectiveEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Objectives.EndPoint.CreateUpdate, async (ObjectiveResponse Data, IRepository Repository) =>
                {
                    Objective? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<Objective, Project>(Data.ProjectId);

                        row = Objective.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<Objective>(Data.Id);
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
            private string[] GetCacheKeys(Objective row)
            {
                List<string> cacheKeys = [
                   
                    .. StaticClass.Objectives.Cache.Key(row.Id, row.ProjectId)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }

        }



        static Objective Map(this ObjectiveResponse request, Objective row)
        {
            row.Name = request.Name;

            return row;
        }

    }

}
