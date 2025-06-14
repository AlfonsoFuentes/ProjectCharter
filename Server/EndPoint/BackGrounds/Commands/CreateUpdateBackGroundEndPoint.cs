using Server.Database.Entities.ProjectManagements;
using Shared.Models.Backgrounds.Requests;
using System.Threading;

namespace Server.EndPoint.BackGrounds.Commands
{

    public static class CreateUpdateBackGroundEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BackGrounds.EndPoint.CreateUpdate, async (BackGroundResponse Data, IRepository Repository) =>
                {
                    BackGround? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<BackGround, Project>(Data.ProjectId);

                        row = BackGround.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<BackGround>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }

                    row.Name = Data.Name;
                    List<string> cache = [.. StaticClass.BackGrounds.Cache.Key(row.Id, row.ProjectId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());


                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


    }

}
