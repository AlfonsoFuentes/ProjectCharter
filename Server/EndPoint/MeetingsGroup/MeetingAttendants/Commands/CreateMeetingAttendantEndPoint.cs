using DocumentFormat.OpenXml.Spreadsheet;
using Server.Database.Entities.ProjectManagements;
using Shared.Models.MeetingAttendants.Requests;

namespace Server.EndPoint.MeetingsGroup.MeetingAttendants.Commands
{
    public static class CreateMeetingAttendantEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MeetingAttendants.EndPoint.Create, async (CreateMeetingAttendantRequest Data, IRepository Repository) =>
                {
                    var row = MeetingAttendant.Create(Data.MeetingId, Data.StakeHolder!.Id);

                    await Repository.AddAsync(row);


                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row,Data.ProjectId));

                    await ReviewStakeHolderInProject(Data, Repository);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);
                });


            }


            private string[] GetCacheKeys(MeetingAttendant row, Guid ProjectId)
            {
                List<string> cacheKeys = [
                     
                    .. StaticClass.Meetings.Cache.Key(row.MeetingId),
                    .. StaticClass.MeetingAttendants.Cache.Key(row.Id)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
            async Task ReviewStakeHolderInProject(CreateMeetingAttendantRequest Data, IRepository Repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> ProjectIncludes = x => x.Include(x => x.StakeHolders);

                Func<IQueryable<StakeHolder>, IIncludableQueryable<StakeHolder, object>> StakeHolderIncludes = x =>
                x.Include(x => x.RoleInsideProject!);

                Expression<Func<Project, bool>> CriteriaProject = x => x.Id == Data.ProjectId;

                Expression<Func<StakeHolder, bool>> CriteriaStakeHolder = x => x.Id == Data.StakeHolder!.Id;

                var project = await Repository.GetAsync(Includes: ProjectIncludes, Criteria: CriteriaProject);

                var stakeholder = await Repository.GetAsync(Criteria: CriteriaStakeHolder, Includes: StakeHolderIncludes);
                if (project == null || stakeholder == null) return;

                if (!project.StakeHolders.Any(x => x.Id == stakeholder.Id))
                {
                    project.StakeHolders.Add(stakeholder);


                    await Repository.UpdateAsync(project);
                }

                if (stakeholder.RoleInsideProject == null)
                {
                    var roleinsideProject = RoleInsideProject.Create(Data.ProjectId, StakeHolderRoleEnum.Other.Name);
                    await Repository.AddAsync(roleinsideProject);
                    stakeholder.RoleInsideProjectId = roleinsideProject.Id;
                    await Repository.UpdateAsync(stakeholder);
                }

                List<string> cache = [ .. StaticClass.StakeHolders.Cache.Key(stakeholder.Id)];

                var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
            }
        }


    }
}
