using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Responses;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicPipes.Commands
{
    public static class CreateUpdateBasicPipeEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicPipes.EndPoint.CreateUpdate, async (BasicPipeResponse data, IRepository repository) =>
                {
                  

                    BasicPipeItem? row = null!;
                    if (data.Id == Guid.Empty)
                    {
                       
                        row = BasicPipeItem.Create(data.ProjectId, data.PipeId);
                      
                        await repository.AddAsync(row);
                    }
                    else
                    {
                        row = await repository.GetByIdAsync<BasicPipeItem>(data.Id);
                        if (row == null) return Result.Fail(data.NotFound);
                        await repository.UpdateAsync(row);

                    }
                    if (data.ShowDetails && data.Template != null)
                    {
                        if (data.Template.Id == Guid.Empty)
                        {
                            var pipeTemplate = await BasicPipingMapper.AddPipeTemplate(repository, data);
                            row.BasicPipeTemplateId = pipeTemplate.Id;
                        }
                        else
                        {
                            row.BasicPipeTemplateId = data.Template.Id;
                        }




                    }

                    data.Map(row);


                    var result = await repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result, data.Succesfully, data.Fail);
                });
            }
            private string[] GetCacheKeys(BasicPipeItem row)
            {
             
                var budgetitems = StaticClass.BudgetItems.Cache.Key(row.Id, row.ProjectId/*, row.GanttTaskId*/);
                var items = StaticClass.BasicPipes.Cache.Key(row.Id, row.ProjectId);
                var templates = row.BasicPipeTemplateId == null ? new[] { string.Empty } : StaticClass.PipeTemplates.Cache.Key(row.BasicPipeTemplateId!.Value);
                List<string> cacheKeys = [
                     ..budgetitems,
                     ..items,
                     ..templates,

                ];

                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }

        }


    }
}

