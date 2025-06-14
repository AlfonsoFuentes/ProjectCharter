using Server.ExtensionsMethods.Projects;
using Shared.Models.BudgetItems.IndividualItems.Taxs.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.Taxs.Commands
{
    public static class CreateUpdateTaxEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Taxs.EndPoint.CreateUpdate, async (TaxResponse data, IRepository repository) =>
                {
                    var project = await ProjectMapper.GetProject(data.ProjectId, repository);
                    if (project == null) return Result.Fail(data.Fail);

                    Tax? row = null!;
                    if (data.Id == Guid.Empty)
                    {
                        int order = ProjectMapper.GetNextOrder<Tax>(project);
                        row = Tax.Create(project.Id);
                        row.Order = order;
                        await repository.AddAsync(row);
                        foreach (var item in data.TaxItems)
                        {
                            var taxitem = TaxesItem.Create(row.Id, item.BudgetItemId!.Value);
                            await repository.AddAsync(taxitem);

                        }
                    }
                    else
                    {
                        row = await repository.GetByIdAsync<Tax>(data.Id);
                        if (row == null) return Result.Fail(data.NotFound);
                        await repository.UpdateAsync(row);
                        await UpdateTaxesItems(repository, row, data);
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
                var items = StaticClass.Taxs.Cache.Key(row.Id, row.ProjectId);
                List<string> cacheKeys = [
                     ..budgetitems,
                     ..items,
                     //..deliverable,

                ];

                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
            async Task UpdateTaxesItems(IRepository Repository, Tax row, TaxResponse Data)
            {
                if (Data.TaxItems != null)
                {
                    var taxesitems = row.TaxesItems.ToList();
                    foreach (var taxitem in Data.TaxItems)
                    {
                        var taxitemRow = taxesitems.FirstOrDefault(x => x.SelectedId == taxitem.BudgetItemId);
                        if (taxitemRow == null)
                        {
                            taxitemRow = TaxesItem.Create(row.Id, taxitem.BudgetItemId!.Value);


                            await Repository.AddAsync(taxitemRow);
                        }

                    }
                    var nozzlesToRemove = taxesitems.Where(x => !Data.TaxItems.Select(x => x.BudgetItemId).Contains(x.SelectedId)).ToList();
                    await Repository.RemoveRangeAsync(nozzlesToRemove);
                }
                else
                {
                    await Repository.RemoveRangeAsync(row.TaxesItems.ToList());
                }
            }
        }

        static Tax Map(this TaxResponse request, Tax row)
        {
            row.Name = request.Name;
            row.Percentage = request.Percentage;
            row.BudgetUSD = request.BudgetUSD;
            row.IsTaxes = true;
            return row;
        }
    }
}

