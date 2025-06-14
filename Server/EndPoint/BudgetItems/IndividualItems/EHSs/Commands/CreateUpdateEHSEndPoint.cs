using Server.ExtensionsMethods.Projects;
using Shared.Models.BudgetItems.IndividualItems.EHSs.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.EHSs.Commands
{
    public static class CreateUpdateEHSEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.EHSs.EndPoint.CreateUpdate, async (EHSResponse data, IRepository repository) =>
                {
                    var project = await ProjectMapper.GetProject(data.ProjectId, repository);
                    if (project == null) return Result.Fail(data.Fail);

                    EHS? row = null!;
                    if (data.Id == Guid.Empty)
                    {
                        int order = ProjectMapper.GetNextOrder<EHS>(project);
                        row = EHS.Create(project.Id);
                        row.Order = order;
                        await repository.AddAsync(row);
                    }
                    else
                    {
                        row = await repository.GetByIdAsync<EHS>(data.Id);
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
                var items = StaticClass.EHSs.Cache.Key(row.Id, row.ProjectId);
                List<string> cacheKeys = [
                     ..budgetitems,
                     ..items,
                     //..deliverable,

                ];

                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }

        }

        static EHS Map(this EHSResponse request, EHS row)
        {
            row.Name = request.Name;
            row.UnitaryCost = request.UnitaryCost;

            row.Quantity = request.Quantity;
            row.BudgetUSD = request.BudgetUSD;
            return row;

        }
    }
}

