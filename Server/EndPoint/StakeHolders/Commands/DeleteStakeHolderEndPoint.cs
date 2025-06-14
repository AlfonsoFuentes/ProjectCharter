using Shared.Models.StakeHolders.Requests;

namespace Server.EndPoint.StakeHolders.Commands
{
    public static class DeleteStakeHolderEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.StakeHolders.EndPoint.Delete, async (DeleteStakeHolderRequest Data, IRepository Repository) =>
                {
                    Func<IQueryable<StakeHolder>, IIncludableQueryable<StakeHolder, object>> Includes = x => x
                   
                    
                      .Include(x => x.MeetingAttendants)
                      .Include(x => x.Judgements)
                      .Include(x => x.RequirementRequestedBys)
                      .Include(x => x.RequirementResponsibles)
                     ;
                    Expression<Func<StakeHolder, bool>> Criteria = x => x.Id == Data.Id;
                    var row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);
                    if (row == null) { return Result.Fail(Data.NotFound); }

                   
                    foreach (var rowintem in row.MeetingAttendants)
                    {
                        rowintem.StakeHolderId = null;
                        await Repository.UpdateAsync(rowintem);
                    }
                    foreach (var rowintem in row.Judgements)
                    {
                        rowintem.ExpertId = null;
                        await Repository.UpdateAsync(rowintem);
                    }
                    foreach (var rowintem in row.RequirementRequestedBys)
                    {
                        rowintem.RequestedById = null;
                        await Repository.UpdateAsync(rowintem);
                    }
                    foreach (var rowintem in row.RequirementResponsibles)
                    {
                        rowintem.ResponsibleId = null;
                        await Repository.UpdateAsync(rowintem);
                    }

                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> IncludesProject = x => x
                     .Include(x => x.StakeHolders).ThenInclude(x => x.RoleInsideProject!)
                     ;
                    Expression<Func<Project, bool>> CriteriaProject = x => x.StakeHolders.Any(x => x.Id == Data.Id);


                    var projects = await Repository.GetAllAsync(Includes: IncludesProject, Criteria: CriteriaProject);

                    List<string> cache = [.. StaticClass.StakeHolders.Cache.Key(row.Id)];

                    projects.ForEach(async x =>
                    {
                        x.StakeHolders.RemoveAll(x => x.Id == Data.Id);
                    
                        await Repository.UpdateAsync(x);
                    });


                    await Repository.RemoveAsync(row);



                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
}
