using Server.Database.Entities.ProjectManagements;
using Shared.Models.OtherTasks.Requests;
using Shared.Models.OtherTasks.Responses;

namespace Server.EndPoint.OtherTasks.Commands
{

    public static class CreateUpdateOtherTaskEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.OtherTasks.EndPoint.CreateUpdate, async (OtherTaskResponse Data, IRepository Repository) =>
                {

                    OtherTask? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<OtherTask, Project>(Data.ProjectId);

                        row = OtherTask.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<OtherTask>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }



                    Data.Map(row);
                    List<string> cache = [.. StaticClass.OtherTasks.Cache.Key(row.Id, row.ProjectId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        static OtherTask Map(this OtherTaskResponse request, OtherTask row)
        {
            row.Name = request.Name;
            //row.Status = request.Status.Id;
            row.StartDate = request.StartDate;
            row.EndDate = request.EndDate;
            return row;
        }

    }

}
