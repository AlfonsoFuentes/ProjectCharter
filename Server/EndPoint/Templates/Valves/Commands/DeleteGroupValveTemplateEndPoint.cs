using Shared.Models.Templates.Valves.Requests;

namespace Server.EndPoint.Templates.Valves.Commands
{
    public static class DeleteGroupValveTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ValveTemplates.EndPoint.DeleteGroup, async (DeleteGroupValveTemplatesRequest Data, IRepository Repository) =>
                {
                    List<string> cache = new List<string>();
                    Func<IQueryable<ValveTemplate>, IIncludableQueryable<ValveTemplate, object>> Includes = x => x
                    .Include(x => x.BasicValves);
                    foreach (var rowItem in Data.SelecteItems)
                    {

                        Expression<Func<ValveTemplate, bool>> Criteria = x => x.Id == rowItem.Id;

                        var row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);
                        if (row != null)
                        {
                            foreach (var item in row.BasicValves)
                            {
                                item.BasicValveTemplateId = null;
                                await Repository.UpdateAsync(item);
                                cache.AddRange(StaticClass.BasicValves.Cache.Key(item.Id,item.ProjectId));
                            }
                            await Repository.RemoveAsync(row);
                        }
                       
                    }


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
