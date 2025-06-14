using Shared.Models.Templates.Pipings.Requests;

namespace Server.EndPoint.Templates.Pipes.Commands
{
    public static class DeleteGroupPipeTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PipeTemplates.EndPoint.DeleteGroup, async (DeleteGroupPipeTemplatesRequest Data, IRepository Repository) =>
                {
                    List<string> cache = new List<string>();
                    Func<IQueryable<PipeTemplate>, IIncludableQueryable<PipeTemplate, object>> Includes = x => x
                    .Include(x => x.BasicPipeItems);
                    foreach (var rowItem in Data.SelecteItems)
                    {

                        Expression<Func<PipeTemplate, bool>> Criteria = x => x.Id == rowItem.Id;

                        var row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);
                        if (row != null)
                        {
                            foreach (var item in row.BasicPipeItems)
                            {
                                item.BasicPipeTemplateId = null;
                                await Repository.UpdateAsync(item);
                                cache.AddRange(StaticClass.Pipes.Cache.Key(item.Id, item.ProjectId));
                            }
                            await Repository.RemoveAsync(row);
                        }
                    }


                    cache.Add(StaticClass.PipeTemplates.Cache.GetAll);

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }

}
