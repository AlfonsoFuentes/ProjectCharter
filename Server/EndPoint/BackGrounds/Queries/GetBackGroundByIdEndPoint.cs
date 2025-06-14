using Server.Database.Entities.ProjectManagements;
using Server.EndPoint.Projects.Queries;
using Shared.Models.Backgrounds.Records;

namespace Server.EndPoint.BackGrounds.Queries
{
    public static class GetBackGroundByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BackGrounds.EndPoint.GetById, async (GetBackgroundByIdRequest request, IQueryRepository Repository) =>
                {
                    //Func<IQueryable<BackGround>, IIncludableQueryable<BackGround, object>> Includes = x => null!
               
                    //;

                    Expression<Func<BackGround, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.BackGrounds.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map();
                    return Result.Success(response);

                });
            }
        }
        public static BackGroundResponse Map(this BackGround row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                ProjectId = row.ProjectId,
                Order = row.Order,
            };
        }


    }
}
