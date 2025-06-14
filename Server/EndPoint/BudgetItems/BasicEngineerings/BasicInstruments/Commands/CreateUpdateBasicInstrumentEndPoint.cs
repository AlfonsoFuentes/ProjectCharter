using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Server.ExtensionsMethods.InstrumentTemplateMapper;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Responses;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicInstruments.Commands
{
    public static class CreateUpdateBasicInstrumentEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicInstruments.EndPoint.CreateUpdate, async (BasicInstrumentResponse data, IRepository repository) =>
                {


                    BasicInstrumentItem? row = null!;
                    if (data.Id == Guid.Empty)
                    {
                       
                        row = BasicInstrumentItem.Create(data.ProjectId,data.InstrumentId);
                       
                        await repository.AddAsync(row);
                        await NozzleMapper.CreateNozzles(repository, row.Id, data.Nozzles);
                    }
                    else
                    {
                        Func<IQueryable<BasicInstrumentItem>, IIncludableQueryable<BasicInstrumentItem, object>> Includes = x => x.Include(x => x.Nozzles);

                        Expression<Func<BasicInstrumentItem, bool>> Criteria = x => x.Id == data.Id;

                        row = await repository.GetAsync(Criteria: Criteria, Includes: Includes);

                        if (row == null) return Result.Fail(data.NotFound);
                        await repository.UpdateAsync(row);
                        await NozzleMapper.UpdateNozzles(repository, row.Nozzles, data.Nozzles, row.Id);
                    }
                    if (data.ShowDetails && data.Template != null)
                    {
                        if (data.Template.Id == Guid.Empty)
                        {
                            var instrumentTemplate = await BasicInstrumentTemplateMapper.AddInstrumentTemplate(repository, data);
                            row.BasicInstrumentTemplateId = instrumentTemplate.Id;
                        }
                        else
                        {
                            row.BasicInstrumentTemplateId = data.Template.Id;
                        }


                    }

                    data.Map(row);


                    var result = await repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result, data.Succesfully, data.Fail);
                });
            }
            private string[] GetCacheKeys(BasicInstrumentItem row)
            {
               
                var budgetitems = StaticClass.BudgetItems.Cache.Key(row.Id, row.ProjectId/*, row.GanttTaskId*/);
                var items = StaticClass.BasicInstruments.Cache.Key(row.Id, row.ProjectId);
                var templates = row.BasicInstrumentTemplateId == null ? new[] { string.Empty } : StaticClass.InstrumentTemplates.Cache.Key(row.BasicInstrumentTemplateId!.Value);
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

