
using Server.Database.Entities.ProjectManagements;
using Shared.Models.Objectives.Records;
using Shared.Models.Objectives.Responses;

namespace Server.EndPoint.Objectives.Queries
{
    public static class GetObjectiveByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Objectives.EndPoint.GetById, async (GetObjectiveByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Objective>, IIncludableQueryable<Objective, object>> Includes = x => null!;


                    Expression<Func<Objective, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Objectives.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria, Includes: Includes);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map();
                    return Result.Success(response);

                });
            }
        }
        public static ObjectiveResponse Map(this Objective row)
        {
            ObjectiveResponse result = new()
            {
                Id = row.Id,
                Name = row.Name,
                ProjectId = row.ProjectId,
                
                
                Order = row.Order,
            };

            return result;
        }



    }
}
