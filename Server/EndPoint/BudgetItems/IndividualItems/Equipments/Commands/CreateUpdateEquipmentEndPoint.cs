using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Equipments;
using Server.ExtensionsMethods.Projects;
using Shared.Models.BudgetItems.IndividualItems.Equipments.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.Equipments.Commands
{
    public static class CreateUpdateEquipmentEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Equipments.EndPoint.CreateUpdate, async (EquipmentResponse data, IRepository repository) =>
                {
                    var project = await ProjectMapper.GetProject(data.ProjectId, repository);
                    if (project == null) return Result.Fail(data.Fail);

                    Equipment? row = null!;
                    if (data.Id == Guid.Empty)
                    {
                        int order = ProjectMapper.GetNextOrder<Equipment>(project);
                        row = Equipment.Create(project.Id);
                        row.Order = order;
                        await repository.AddAsync(row);
                        //await NozzleMapper.CreateNozzles(repository, row.Id, data.Nozzles);
                    }
                    else
                    {
                        Func<IQueryable<Equipment>, IIncludableQueryable<Equipment, object>> Includes = x => x.Include(x => x.EquipmentItems);

                        Expression<Func<Equipment, bool>> Criteria = x => x.Id == data.Id;

                        row = await repository.GetAsync(Criteria: Criteria, Includes: Includes);
            
                        if (row == null) return Result.Fail(data.NotFound);
                        
                        await repository.UpdateAsync(row);
                        
                        //await NozzleMapper.UpdateNozzles(repository, row.Nozzles, data.Nozzles, row.Id);
                    }
                    //if (data.ShowDetails && data.Template != null)
                    //{
                    //    if (data.Template.Id == Guid.Empty)
                    //    {
                    //        var equipmentTemplate = await EquipmentTemplateMapper.AddEquipmentTemplate(repository, data);
                    //        row.EquipmentTemplateId = equipmentTemplate.Id;
                    //    }
                    //    else
                    //    {
                    //        row.EquipmentTemplateId = data.Template.Id;
                    //    }

                    //}

                    data.Map(row);


                    var result = await repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result, data.Succesfully, data.Fail);
                });
            }
            private string[] GetCacheKeys(Equipment row)
            {
               // var deliverable = row.GanttTaskId.HasValue ? StaticClass.GanttTasks.Cache.Key(row.GanttTaskId!.Value, row.ProjectId) : new[] { string.Empty };
                var budgetitems = StaticClass.BudgetItems.Cache.Key(row.Id, row.ProjectId/*, row.GanttTaskId*/);
                var items = StaticClass.Equipments.Cache.Key(row.Id, row.ProjectId);
                //var templates = row.EquipmentTemplateId == null ? new[] { string.Empty } : StaticClass.EquipmentTemplates.Cache.Key(row.EquipmentTemplateId!.Value);
                List<string> cacheKeys = [
                     ..budgetitems,
                     ..items,
                     //..deliverable,
                     //..templates,

                ];

                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }

        }


    }
}

