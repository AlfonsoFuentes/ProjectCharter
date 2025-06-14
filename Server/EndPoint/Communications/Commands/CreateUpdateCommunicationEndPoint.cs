using Shared.Models.Communications.Responses;

namespace Server.EndPoint.Communications.Commands
{

    public static class CreateUpdateCommunicationEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Communications.EndPoint.CreateUpdate, async (CommunicationResponse Data, IRepository Repository) =>
                {

                    Communication? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<Communication, Project>(Data.ProjectId);

                        row = Communication.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<Communication>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }



                    Data.Map(row);
                    List<string> cache = [.. StaticClass.Communications.Cache.Key(row.Id, row.ProjectId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        static Communication Map(this CommunicationResponse request, Communication row)
        {
            row.Name = request.Name;
            return row;
        }

    }

}
