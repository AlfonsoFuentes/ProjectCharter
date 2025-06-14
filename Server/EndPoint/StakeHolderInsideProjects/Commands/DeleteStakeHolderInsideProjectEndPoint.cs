using Shared.Models.StakeHolderInsideProjects.Requests;

namespace Server.EndPoint.StakeHolderInsideProjects.Commands
{
    public static class DeleteStakeHolderInsideProjectEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.StakeHolderInsideProjects.EndPoint.Delete, async (DeleteStakeHolderInsideProjectRequest Data, IRepository Repository) =>
                {
                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> ProjectIncludes = x => x.Include(x => x.StakeHolders);
                    Expression<Func<Project, bool>> CriteriaProject = x => x.Id == Data.ProjectId;

                    var project = await Repository.GetAsync(Includes: ProjectIncludes, Criteria: CriteriaProject);

                    var stakeholder = await Repository.GetByIdAsync<StakeHolder>(Data.Id);

                    if (project == null || stakeholder == null) { return Result.Fail(Data.NotFound); }

                    project.StakeHolders.Remove(stakeholder);

                    await Repository.UpdateAsync(project);

                    List<string> cache = [.. StaticClass.StakeHolderInsideProjects.Cache.Key(stakeholder.Id, project.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }

}
