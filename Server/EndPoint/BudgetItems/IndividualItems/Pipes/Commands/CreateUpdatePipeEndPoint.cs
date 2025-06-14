using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Pipings;
using Server.ExtensionsMethods.Projects;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.Pipes.Commands
{
    public static class CreateUpdatePipeEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Pipes.EndPoint.CreateUpdate, async (PipeResponse data, IRepository repository) =>
                {
                    var project = await ProjectMapper.GetProject(data.ProjectId, repository);
                    if (project == null) return Result.Fail(data.Fail);

                    Pipe? row = null!;
                    if (data.Id == Guid.Empty)
                    {
                        int order = ProjectMapper.GetNextOrder<Pipe>(project);
                        row = Pipe.Create(project.Id);
                        row.Order = order;
                        await repository.AddAsync(row);
                    }
                    else
                    {
                        row = await repository.GetByIdAsync<Pipe>(data.Id);
                        if (row == null) return Result.Fail(data.NotFound);
                        await repository.UpdateAsync(row);

                    }
                    //if (data.ShowDetails && data.Template != null)
                    //{
                    //    if (data.Template.Id == Guid.Empty)
                    //    {
                    //        var pipeTemplate = await PipingMapper.AddPipeTemplate(repository, data);
                    //        row.PipeTemplateId = pipeTemplate.Id;
                    //    }
                    //    else
                    //    {
                    //       row.PipeTemplateId = data.Template.Id;
                    //    }

                        


                    //}

                    data.Map(row);


                    var result = await repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result, data.Succesfully, data.Fail);
                });
            }
            private string[] GetCacheKeys(Pipe row)
            {
               // var deliverable = row.GanttTaskId.HasValue ? StaticClass.GanttTasks.Cache.Key(row.GanttTaskId!.Value, row.ProjectId) : new[] { string.Empty };
                var budgetitems = StaticClass.BudgetItems.Cache.Key(row.Id, row.ProjectId/*, row.GanttTaskId*/);
                var items = StaticClass.Pipes.Cache.Key(row.Id, row.ProjectId);
                //var templates = row.PipeTemplateId == null ? new[] { string.Empty } : StaticClass.PipeTemplates.Cache.Key(row.PipeTemplateId!.Value);
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

