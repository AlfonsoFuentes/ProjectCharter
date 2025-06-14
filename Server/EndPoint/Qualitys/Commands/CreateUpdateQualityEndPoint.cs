using Server.Database.Entities.ProjectManagements;
using Shared.Models.Qualitys.Requests;
using Shared.Models.Qualitys.Responses;

namespace Server.EndPoint.Qualitys.Commands
{

    public static class CreateUpdateQualityEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Qualitys.EndPoint.CreateUpdate, async (QualityResponse Data, IRepository Repository) =>
                {
                    Quality? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<Quality, Project>(Data.ProjectId);

                        row = Quality.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<Quality>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }




                    Data.Map(row);
                    List<string> cache = [.. StaticClass.Qualitys.Cache.Key(row.Id, row.ProjectId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
            private string[] GetCacheKeys(Quality row)
            {
                List<string> cacheKeys = [
                    .. StaticClass.Qualitys.Cache.Key(row.Id, row.ProjectId)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }


        static Quality Map(this QualityResponse request, Quality row)
        {
            row.Name = request.Name;
            return row;
        }

    }

}
