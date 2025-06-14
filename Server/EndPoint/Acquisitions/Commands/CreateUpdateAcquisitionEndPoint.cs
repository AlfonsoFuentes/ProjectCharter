using Server.Database.Entities.ProjectManagements;
using Shared.Models.Acquisitions.Requests;
using Shared.Models.Acquisitions.Responses;

namespace Server.EndPoint.Acquisitions.Commands
{

    public static class CreateUpdateAcquisitionEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Acquisitions.EndPoint.CreateUpdate, async (AcquisitionResponse Data, IRepository Repository) =>
                {

                    Acquisition? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<Acquisition, Project>(Data.ProjectId);

                        row = Acquisition.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<Acquisition>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }



                    Data.Map(row);
                    List<string> cache = [.. StaticClass.Acquisitions.Cache.Key(row.Id, row.ProjectId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        static Acquisition Map(this AcquisitionResponse request, Acquisition row)
        {
            row.Name = request.Name;
            return row;
        }

    }

}
