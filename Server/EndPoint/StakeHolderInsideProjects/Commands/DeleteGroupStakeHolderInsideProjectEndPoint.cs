using Shared.Models.StakeHolderInsideProjects.Requests;

namespace Server.EndPoint.StakeHolderInsideProjects.Commands
{
    public static class DeleteGroupStakeHolderInsideProjectEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.StakeHolderInsideProjects.EndPoint.DeleteGroup, async (DeleteGroupStakeHolderInsideProjectRequest Data, IRepository Repository) =>
                {
                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> ProjectIncludes = x => x.Include(x => x.StakeHolders);
                    Expression<Func<Project, bool>> CriteriaProject = x => x.Id == Data.ProjectId;
                    var project = await Repository.GetAsync(Includes: ProjectIncludes, Criteria: CriteriaProject);
                    if (project == null) { return Result.Fail(Data.NotFound); }
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var stakeholder = await Repository.GetByIdAsync<StakeHolder>(rowItem.Id);
                        if (stakeholder != null && project.StakeHolders.Any(x => x.Id == stakeholder.Id))
                        {
                            project.StakeHolders.Remove(stakeholder);

                            await Repository.UpdateAsync(project);
                        }


                    }


                    var cache = StaticClass.StakeHolderInsideProjects.Cache.GetAll(Data.ProjectId);

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }

}
