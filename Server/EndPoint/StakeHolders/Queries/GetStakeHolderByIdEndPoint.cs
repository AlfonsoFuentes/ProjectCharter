using Server.Database.Entities.ProjectManagements;
using Server.EndPoint.Projects.Queries;
using Shared.Models.StakeHolders.Records;

namespace Server.EndPoint.StakeHolders.Queries
{

    public static class GetStakeHolderByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.StakeHolders.EndPoint.GetById, async (GetStakeHolderByIdRequest request, IQueryRepository Repository) =>
                {

                    Expression<Func<StakeHolder, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.StakeHolders.Cache.GetById(request.Id);
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

        public static StakeHolderResponse Map(this StakeHolder row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                Area = row.Area,
                Email = row.Email,
                PhoneNumber = row.PhoneNumber,


            };
        }

    }
}
