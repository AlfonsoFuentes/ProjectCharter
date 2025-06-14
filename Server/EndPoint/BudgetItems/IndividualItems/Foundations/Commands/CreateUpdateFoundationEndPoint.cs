using Server.ExtensionsMethods.Projects;
using Shared.Models.BudgetItems.IndividualItems.Foundations.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.Foundations.Commands
{
    public static class CreateUpdateFoundationEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Foundations.EndPoint.CreateUpdate, async (FoundationResponse data, IRepository repository) =>
                {
                    var project = await ProjectMapper.GetProject(data.ProjectId, repository);
                    if (project == null) return Result.Fail(data.Fail);

                    Foundation? row = null!;
                    if (data.Id == Guid.Empty)
                    {
                        int order = ProjectMapper.GetNextOrder<Foundation>(project);
                        row = Foundation.Create(project.Id);
                        row.Order = order;
                        await repository.AddAsync(row);
                    }
                    else
                    {
                        row = await repository.GetByIdAsync<Foundation>(data.Id);
                        if (row == null) return Result.Fail(data.NotFound);

                        await repository.UpdateAsync(row);
                    }


                    data.Map(row);


                    var result = await repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result, data.Succesfully, data.Fail);
                });
            }
            private string[] GetCacheKeys(BudgetItem row)
            {
               // var deliverable = row.GanttTaskId.HasValue ? StaticClass.GanttTasks.Cache.Key(row.GanttTaskId!.Value, row.ProjectId) : new[] { string.Empty };
                var budgetitems = StaticClass.BudgetItems.Cache.Key(row.Id, row.ProjectId/*, row.GanttTaskId*/);
                var items = StaticClass.Foundations.Cache.Key(row.Id, row.ProjectId);
                List<string> cacheKeys = [
                     ..budgetitems,
                     ..items,
                     //..deliverable,

                ];

                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }

        }

        static Foundation Map(this FoundationResponse request, Foundation row)
        {
            row.Name = request.Name;
            row.UnitaryCost = request.UnitaryCost;

            row.Quantity = request.Quantity;
            row.BudgetUSD = request.BudgetUSD;
            return row;
        }
    }
}

