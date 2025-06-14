using Server.Database.Entities.ProjectManagements;
using Shared.Models.StakeHolderInsideProjects.Requests;
using Shared.Models.StakeHolderInsideProjects.Responses;

namespace Server.EndPoint.StakeHolderInsideProjects.Commands
{

    public static class CreateStakeHolderInsideProjectEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.StakeHolderInsideProjects.EndPoint.Create, async (StakeHolderInsideProjectResponse Data, IRepository Repository) =>
                {
                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> ProjectIncludes = x => x.Include(x => x.StakeHolders);

                    Func<IQueryable<StakeHolder>, IIncludableQueryable<StakeHolder, object>> StakeHolderIncludes = x =>
                    x.Include(x => x.RoleInsideProject!);

                    Expression<Func<Project, bool>> CriteriaProject = x => x.Id == Data.ProjectId;

                    Expression<Func<StakeHolder, bool>> CriteriaStakeHolder = x => x.Id == Data.StakeHolder!.Id;

                    var project = await Repository.GetAsync(Includes: ProjectIncludes, Criteria: CriteriaProject);

                    var stakeholder = await Repository.GetAsync(Criteria: CriteriaStakeHolder, Includes: StakeHolderIncludes);

                    if (project == null || stakeholder == null) return Result.Fail();

                    if (!project.StakeHolders.Any(x => x.Id == stakeholder.Id))
                    {
                        project.StakeHolders.Add(stakeholder);


                        await Repository.UpdateAsync(project);
                    }
                    else
                    {
                        return Result.Fail($"Stake Holder {Data.StakeHolder!.Name} is exist in project {project.Name}");
                    }
                    if (stakeholder.RoleInsideProject == null)
                    {
                        var roleinsideProject = RoleInsideProject.Create(Data.ProjectId, Data.Role.Name);
                        await Repository.AddAsync(roleinsideProject);
                        stakeholder.RoleInsideProjectId = roleinsideProject.Id;
                        await Repository.UpdateAsync(stakeholder);
                    }

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
