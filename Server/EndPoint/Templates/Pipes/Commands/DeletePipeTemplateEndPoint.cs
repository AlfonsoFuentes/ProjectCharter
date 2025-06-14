using Shared.Models.Templates.Pipings.Requests;

namespace Server.EndPoint.Templates.Pipes.Commands
{
    public static class DeletePipeTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PipeTemplates.EndPoint.Delete, async (DeletePipeTemplateRequest Data, IRepository Repository) =>
                {
                    Func<IQueryable<PipeTemplate>, IIncludableQueryable<PipeTemplate, object>> Includes = x => x
                    .Include(x => x.BasicPipeItems);
                    Expression<Func<PipeTemplate, bool>> Criteria = x => x.Id == Data.Id;

                    var row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);

                    if (row == null) { return Result.Fail(Data.NotFound); }
                    List<string> cache = new List<string>();
                    foreach (var item in row.BasicPipeItems)
                    {
                        item.BasicPipeTemplateId = null;
                        await Repository.UpdateAsync(item);
                        cache.AddRange(StaticClass.Pipes.Cache.Key(item.Id, item.ProjectId));
                    }
                    await Repository.RemoveAsync(row);
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
