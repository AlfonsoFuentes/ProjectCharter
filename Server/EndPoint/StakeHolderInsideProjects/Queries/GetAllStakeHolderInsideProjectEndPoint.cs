using Shared.Models.Backgrounds.Records;
using Shared.Models.StakeHolderInsideProjects.Records;
using Shared.Models.StakeHolderInsideProjects.Responses;

namespace Server.EndPoint.StakeHolderInsideProjects.Queries
{
    public static class GetAllStakeHolderInsideProjectEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.StakeHolderInsideProjects.EndPoint.GetAll, async (StakeHolderInsideProjectGetAll request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> Includes = x => x
                    .Include(x => x.StakeHolders)
                    .ThenInclude(x => x.RoleInsideProject!)
                    ;

                    Expression<Func<Project, bool>> Criteria = x => x.Id == request.ProjectId;



                    string CacheKey = StaticClass.StakeHolderInsideProjects.Cache.GetAll(request.ProjectId);
                    var rows = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria, Includes: Includes);

                    if (rows == null)
                    {
                        return Result<StakeHolderInsideProjectResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.StakeHolderInsideProjects.ClassLegend));
                    }

                    var maps = rows.StakeHolders.Select(x => x.MapInsideProject(request.ProjectId)).ToList();


                    StakeHolderInsideProjectResponseList response = new StakeHolderInsideProjectResponseList()
                    {
                        Items = maps,
                        ProjectId = request.ProjectId,
                    };
                    return Result<StakeHolderInsideProjectResponseList>.Success(response);

                });
            }
        }
    }
}