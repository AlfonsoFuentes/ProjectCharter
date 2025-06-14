using Server.Database.Entities.BudgetItems.Commons;
using Shared.Models.BudgetItems.IndividualItems.Alterations.Validators;

namespace Server.EndPoint.BudgetItems.IndividualItems.Alterations.Validators
{
    public static class ValidateAlterationsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Alterations.EndPoint.Validate, async (ValidateAlterationRequest data, IQueryRepository repository) =>
                {
                    try
                    {
                        if (data == null || string.IsNullOrEmpty(data.Name) || data.ProjectId == Guid.Empty)
                        {
                            return Result.Fail("Invalid request data.");
                        }

                        Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.BudgetItems);
                        Expression<Func<Project, bool>> criteria = p => p.Id == data.ProjectId;
                        string cacheKey = StaticClass.Projects.Cache.GetById(data.ProjectId);

                        var project = await repository.GetAsync(Cache: cacheKey, Criteria: criteria, Includes: includes);
                        if (project == null || project.BudgetItems == null)
                        {
                            return Result.Fail("Project or budget items not found.");
                        }

                        Func<BudgetItem, bool> criteriaExist = x => data.Id == null ?
                            x.Name.Equals(data.Name, StringComparison.OrdinalIgnoreCase) :
                            x.Id != data.Id.Value && x.Name.Equals(data.Name, StringComparison.OrdinalIgnoreCase);

                        bool result = project.BudgetItems.Any(criteriaExist);

                        return Result.Success(result);
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (logging mechanism not shown here)
                        return Result.Fail(ex.Message);
                    }
                });
            }
        }
    }

}
