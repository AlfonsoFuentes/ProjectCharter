using Shared.Models.StakeHolderInsideProjects.Validators;

namespace Server.EndPoint.StakeHolderInsideProjects.Validators
{
    public static class ValidateStakeHolderInsideProjectNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.StakeHolderInsideProjects.EndPoint.Validate, async (ValidateStakeHolderInsideProjectRequest Data, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> ProjectIncludes = x => x.Include(x => x.StakeHolders);

                    Expression<Func<Project, bool>> CriteriaId = x => x.Id == Data.ProjectId;

                    Func<Project, bool> CriteriaExist = x => Data.OriginalStakeHolderId == null ? 
                    x.StakeHolders.Any(x => x.Id == Data.StakeHolderId) :
                    x.StakeHolders.Where(x => x.Id != Data.OriginalStakeHolderId.Value).Any(x => x.Id == Data.StakeHolderId)
                    ;

                    string CacheKey = StaticClass.Projects.Cache.GetValidateById(Data.ProjectId);

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId, Includes: ProjectIncludes);
                });


            }
        }



    }

}
