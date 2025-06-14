using Server.Database.Entities.ProjectManagements;
using Server.EndPoint.Projects.Queries;
using Shared.Models.StakeHolderInsideProjects.Records;
using Shared.Models.StakeHolderInsideProjects.Responses;

namespace Server.EndPoint.StakeHolderInsideProjects.Queries
{
    public static class GetStakeHolderInsideProjectByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.StakeHolderInsideProjects.EndPoint.GetById, async (GetStakeHolderInsideProjectByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<StakeHolder>, IIncludableQueryable<StakeHolder, object>> Includes = x => x.Include(x => x.RoleInsideProject!);

                    Expression<Func<StakeHolder, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.StakeHolders.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria, Includes: Includes);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.MapWithRole(request.ProjectId);
                    return Result.Success(response);

                });
            }

        }
        static StakeHolderInsideProjectResponse MapWithRole(this StakeHolder row, Guid ProjectId)
        {
            return new StakeHolderInsideProjectResponse()
            {
                StakeHolder = new()
                {
                    Id = row.Id,
                    Name = row.Name,
                    Area = row.Area,
                    Email = row.Email,
                    PhoneNumber = row.PhoneNumber,
                },
                ProjectId = ProjectId,
                Role = row.RoleInsideProject == null ? StakeHolderRoleEnum.None : StakeHolderRoleEnum.GetType(row.RoleInsideProject.Name)

            };

        }
        public static StakeHolderInsideProjectResponse MapInsideProject(this StakeHolder row, Guid _projectid)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                StakeHolder = new()
                {
                    Id = row.Id,
                    Name = row.Name,
                    PhoneNumber = row.PhoneNumber,
                    Email = row.Email,
                    Area = row.Area,

                },
                OriginalStakeHolderId = row.Id,
                ProjectId = _projectid,
                Role = row.RoleInsideProject == null ? StakeHolderRoleEnum.None : StakeHolderRoleEnum.GetType(row.RoleInsideProject.Name),



            };
        }

    }
}
