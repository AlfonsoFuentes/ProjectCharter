using Server.Database.Entities.ProjectManagements;
using Shared.Models.StakeHolderInsideProjects.Requests;
using Shared.Models.StakeHolderInsideProjects.Responses;
using static Shared.StaticClasses.StaticClass;

namespace Server.EndPoint.StakeHolderInsideProjects.Commands
{
    public static class UpdateStakeHolderInsideProjectEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.StakeHolderInsideProjects.EndPoint.Update, async (StakeHolderInsideProjectResponse Data, IRepository Repository) =>
                {
                    Expression<Func<StakeHolder, bool>> CriteriaStakeHolder = x => x.Id == Data.StakeHolder!.Id;

                    Func<IQueryable<StakeHolder>, IIncludableQueryable<StakeHolder, object>> StakeHolderIncludes = x =>
                   x.Include(x => x.RoleInsideProject!);

                    var row = await Repository.GetAsync(Criteria: CriteriaStakeHolder, Includes: StakeHolderIncludes);

                    if (row == null || row!.RoleInsideProject == null) return Result.Fail(Data.Fail);

                    var rolinsideproject = await Repository.GetByIdAsync<RoleInsideProject>(row.RoleInsideProjectId!.Value);

                    if (rolinsideproject == null) return Result.Fail(Data.Fail);

                    rolinsideproject.Name = Data.Role.Name;
                    await Repository.UpdateAsync(rolinsideproject);

                    List<string> cache = [.. StaticClass.StakeHolderInsideProjects.Cache.Key(row.Id, Data.ProjectId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });
            }
        }


    }

}
