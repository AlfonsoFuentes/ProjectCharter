using DocumentFormat.OpenXml.Drawing.Charts;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Valves;
using Server.ExtensionsMethods.Projects;
using Shared.Models.BudgetItems.IndividualItems.Valves.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.Valves.Commands
{
    public static class CreateUpdateValveEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Valves.EndPoint.CreateUpdate, async (ValveResponse data, IRepository repository) =>
                {
                    var project = await ProjectMapper.GetProject(data.ProjectId, repository);
                    if (project == null) return Result.Fail(data.Fail);

                    Valve? row = null!;
                    if (data.Id == Guid.Empty)
                    {
                        int order = ProjectMapper.GetNextOrder<Valve>(project);
                        row = Valve.Create(project.Id);
                        row.Order = order;
                        await repository.AddAsync(row);
                        //await NozzleMapper.CreateNozzles(repository, row.Id, data.Nozzles);
                    }
                    else
                    {
                        Func<IQueryable<Valve>, IIncludableQueryable<Valve, object>> Includes = x => x.Include(x => x.ValveItems);

                        Expression<Func<Valve, bool>> Criteria = x => x.Id == data.Id;

                        row = await repository.GetAsync(Criteria: Criteria, Includes: Includes);
                      
                        if (row == null) return Result.Fail(data.NotFound);
                        await repository.UpdateAsync(row);
                        //await NozzleMapper.UpdateNozzles(repository, row.Nozzles, data.Nozzles, row.Id);

                    }
                    //if (data.ShowDetails && data.Template != null)
                    //{
                    //    if(data.Template.Id==Guid.Empty)
                    //    {
                    //        var valveTemplate = await ValveTemplateMapper.AddValveTemplate(repository, data);
                    //        row.ValveTemplateId = valveTemplate.Id;
                    //    }
                    //    else
                    //    {
                    //        row.ValveTemplateId = data.Template.Id;
                    //    }


                    //}

                    data.Map(row);


                    var result = await repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result, data.Succesfully, data.Fail);
                });
            }
            private string[] GetCacheKeys(Valve row)
            {
               // var deliverable = row.GanttTaskId.HasValue ? StaticClass.GanttTasks.Cache.Key(row.GanttTaskId!.Value, row.ProjectId) : new[] { string.Empty };
                var budgetitems = StaticClass.BudgetItems.Cache.Key(row.Id, row.ProjectId/*, row.GanttTaskId*/);
                //var templates = row.ValveTemplateId == null ? new[] { string.Empty } : StaticClass.ValveTemplates.Cache.Key(row.ValveTemplateId!.Value);
                var items = StaticClass.Valves.Cache.Key(row.Id, row.ProjectId);
                List<string> cacheKeys = [
                     ..budgetitems,
                     ..items,
                     //..deliverable,
                    //..templates

                ];

                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }

        }


    }
}

