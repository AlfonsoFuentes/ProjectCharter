using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Server.Database.Entities.ProjectManagements;
using Server.EndPoint.Projects.Queries;
using Shared.Models.StakeHolders.Records;
using Shared.Models.StakeHolders.Responses;

namespace Server.EndPoint.StakeHolders.Queries
{
    public static class GetAllStakeHolderEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.StakeHolders.EndPoint.GetAll, async (StakeHolderGetAll request, IQueryRepository Repository) =>
                {
                   
                    string CacheKey = StaticClass.StakeHolders.Cache.GetAll;
                    var rows = await Repository.GetAllAsync<StakeHolder>(CacheKey);

                    if (rows == null)
                    {
                        return Result<StakeHolderResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.StakeHolders.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    StakeHolderResponseList response = new StakeHolderResponseList()
                    {
                        Items = maps
                    };
                    return Result<StakeHolderResponseList>.Success(response);

                });
            }
        }
    }
}