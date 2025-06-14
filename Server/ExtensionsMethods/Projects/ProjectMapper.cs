namespace Server.ExtensionsMethods.Projects
{
    public static class ProjectMapper
    {
        public static async Task<Project?> GetProject(Guid ProjectId, IRepository repository)
        {
            Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x

                .Include(d => d.BudgetItems)
                ;

            Expression<Func<Project, bool>> criteria = d => d.Id == ProjectId;

            return await repository.GetAsync(Criteria: criteria, Includes: includes);
        }

        public static int GetNextOrder<T>(Project project) where T : BudgetItem
        {
            var items = project.BudgetItems.OfType<T>();
            return items.Any() ? items.Max(i => i.Order) + 1 : 1;
        }
       
    }
}
