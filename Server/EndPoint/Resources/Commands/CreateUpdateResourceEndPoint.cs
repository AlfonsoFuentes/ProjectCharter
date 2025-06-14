using Server.Database.Entities.ProjectManagements;
using Shared.Models.Resources.Requests;
using Shared.Models.Resources.Responses;

namespace Server.EndPoint.Resources.Commands
{

    public static class CreateUpdateResourceEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Resources.EndPoint.CreateUpdate, async (ResourceResponse Data, IRepository Repository) =>
                {
                    Resource? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<Resource, Project>(Data.ProjectId);

                        row = Resource.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<Resource>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }

                   

                    Data.Map(row);
                    List<string> cache = [.. StaticClass.Resources.Cache.Key(row.Id, row.ProjectId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        static Resource Map(this ResourceResponse request, Resource row)
        {
            row.Name = request.Name;
            return row;
        }

    }

}
