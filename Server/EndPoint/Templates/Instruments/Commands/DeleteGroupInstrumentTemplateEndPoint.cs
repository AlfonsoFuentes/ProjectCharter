using Shared.Models.Templates.Instruments.Requests;

namespace Server.EndPoint.Templates.Instruments.Commands
{
    public static class DeleteGroupInstrumentTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InstrumentTemplates.EndPoint.DeleteGroup, async (DeleteGroupInstrumentTemplatesRequest Data, IRepository Repository) =>
                {
                    List<string> cache = new List<string>();
                    Func<IQueryable<InstrumentTemplate>, IIncludableQueryable<InstrumentTemplate, object>> Includes = x => x
                    .Include(x => x.BasicInstruments);
                    foreach (var rowItem in Data.SelecteItems)
                    {

                        Expression<Func<InstrumentTemplate, bool>> Criteria = x => x.Id == rowItem.Id;

                        var row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);
                        if (row != null)
                        {
                            foreach (var item in row.BasicInstruments)
                            {
                                item.BasicInstrumentTemplateId = null;
                                await Repository.UpdateAsync(item);
                                cache.AddRange(StaticClass.BasicInstruments.Cache.Key(item.Id,item.ProjectId));
                            }
                            await Repository.RemoveAsync(row);
                        }
                    }

                    cache.Add(StaticClass.InstrumentTemplates.Cache.GetAll);
                    

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }

}
