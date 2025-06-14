using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Responses;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicEquipments.Commands
{
    public static class CreateUpdateBasicEquipmentEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicEquipments.EndPoint.CreateUpdate, async (BasicEquipmentResponse data, IRepository repository) =>
                {


                    BasicEquipmentItem? row = null!;
                    if (data.Id == Guid.Empty)
                    {
                        
                        row = BasicEquipmentItem.Create(data.ProjectId,data.EquipmentId);
                       
                        await repository.AddAsync(row);
                        await NozzleMapper.CreateNozzles(repository, row.Id, data.Nozzles);
                    }
                    else
                    {
                        Func<IQueryable<BasicEquipmentItem>, IIncludableQueryable<BasicEquipmentItem, object>> Includes = x => x.Include(x => x.Nozzles);

                        Expression<Func<BasicEquipmentItem, bool>> Criteria = x => x.Id == data.Id;

                        row = await repository.GetAsync(Criteria: Criteria, Includes: Includes);
            
                        if (row == null) return Result.Fail(data.NotFound);
                        
                        await repository.UpdateAsync(row);
                        
                        await NozzleMapper.UpdateNozzles(repository, row.Nozzles, data.Nozzles, row.Id);
                    }
                    if (data.ShowDetails && data.Template != null)
                    {
                        if (data.Template.Id == Guid.Empty)
                        {
                            var equipmentTemplate = await BasicEquipmentTemplateMapper.AddEquipmentTemplate(repository, data);
                            row.BasicEquipmentTemplateId = equipmentTemplate.Id;
                        }
                        else
                        {
                            row.BasicEquipmentTemplateId = data.Template.Id;
                        }

                    }

                    data.Map(row);


                    var result = await repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result, data.Succesfully, data.Fail);
                });
            }
            private string[] GetCacheKeys(BasicEquipmentItem row)
            {
              
                var budgetitems = StaticClass.BudgetItems.Cache.Key(row.Id, row.ProjectId/*, row.GanttTaskId*/);
                var items = StaticClass.BasicEquipments.Cache.Key(row.Id, row.ProjectId);
                var templates = row.BasicEquipmentTemplateId == null ? new[] { string.Empty } : StaticClass.EquipmentTemplates.Cache.Key(row.BasicEquipmentTemplateId!.Value);
                List<string> cacheKeys = [
                     ..budgetitems,
                     ..items,
                     //..deliverable,
                     ..templates,

                ];

                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }

        }


    }
}

