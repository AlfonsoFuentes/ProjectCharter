using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Responses;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicValves.Commands
{
    public static class CreateUpdateBasicValveEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicValves.EndPoint.CreateUpdate, async (BasicValveResponse data, IRepository repository) =>
                {
                   

                    BasicValveItem? row = null!;
                    if (data.Id == Guid.Empty)
                    {
                      
                        row = BasicValveItem.Create(data.ProjectId,data.ValveId);
                      
                        await repository.AddAsync(row);
                        await NozzleMapper.CreateNozzles(repository, row.Id, data.Nozzles);
                    }
                    else
                    {
                        Func<IQueryable<BasicValveItem>, IIncludableQueryable<BasicValveItem, object>> Includes = x => x.Include(x => x.Nozzles);

                        Expression<Func<BasicValveItem, bool>> Criteria = x => x.Id == data.Id;

                        row = await repository.GetAsync(Criteria: Criteria, Includes: Includes);
                      
                        if (row == null) return Result.Fail(data.NotFound);
                        await repository.UpdateAsync(row);
                        await NozzleMapper.UpdateNozzles(repository, row.Nozzles, data.Nozzles, row.Id);

                    }
                    if (data.ShowDetails && data.Template != null)
                    {
                        if (data.Template.Id == Guid.Empty)
                        {
                            var valveTemplate = await BasicValveTemplateMapper.AddValveTemplate(repository, data);
                            row.BasicValveTemplateId = valveTemplate.Id;
                        }
                        else
                        {
                            row.BasicValveTemplateId = data.Template.Id;
                        }


                    }

                    data.Map(row);


                    var result = await repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result, data.Succesfully, data.Fail);
                });
            }
            private string[] GetCacheKeys(BasicValveItem row)
            {
              
                var budgetitems = StaticClass.BudgetItems.Cache.Key(row.Id, row.ProjectId/*, row.GanttTaskId*/);
                var templates = row.BasicValveTemplateId == null ? new[] { string.Empty } : StaticClass.ValveTemplates.Cache.Key(row.BasicValveTemplateId!.Value);
                var items = StaticClass.BasicValves.Cache.Key(row.Id, row.ProjectId);
                List<string> cacheKeys = [
                     ..budgetitems,
                     ..items,
                     ..templates

                ];

                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }

        }


    }
}

