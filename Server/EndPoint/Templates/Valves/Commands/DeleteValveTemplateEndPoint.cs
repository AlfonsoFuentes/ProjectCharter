using Shared.Models.Templates.Valves.Requests;

namespace Server.EndPoint.Templates.Valves.Commands
{
    public static class DeleteValveTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ValveTemplates.EndPoint.Delete, async (DeleteValveTemplateRequest Data, IRepository Repository) =>
                {
                    Func<IQueryable<ValveTemplate>, IIncludableQueryable<ValveTemplate, object>> Includes = x => x
                    .Include(x => x.BasicValves);
                    Expression<Func<ValveTemplate, bool>> Criteria = x => x.Id == Data.Id;

                    var row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);

                    if (row == null) { return Result.Fail(Data.NotFound); }

                    List<string> cache = new List<string>();
                    foreach (var item in row.BasicValves)
                    {
                        item.BasicValveTemplateId = null;
                        await Repository.UpdateAsync(item);
                        cache.AddRange(StaticClass.BasicValves.Cache.Key(item.Id, item.ProjectId));
                    }
                    await Repository.RemoveAsync(row);
                    cache.Add(StaticClass.ValveTemplates.Cache.GetById(row.Id));
                    cache.Add(StaticClass.ValveTemplates.Cache.GetAll);
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }

}
