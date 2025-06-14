using Shared.Models.Templates.Instruments.Requests;

namespace Server.EndPoint.Templates.Instruments.Commands
{
    public static class DeleteInstrumentTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InstrumentTemplates.EndPoint.Delete, async (DeleteInstrumentTemplateRequest Data, IRepository Repository) =>
                {
                    try
                    {
                        if (Data == null || Data.Id == Guid.Empty)
                        {
                            return Result.Fail("Invalid request data.");
                        }

                        Func<IQueryable<InstrumentTemplate>, IIncludableQueryable<InstrumentTemplate, object>> Includes = x => x
                            .Include(x => x.BasicInstruments);
                        Expression<Func<InstrumentTemplate, bool>> Criteria = x => x.Id == Data.Id;

                        var row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);

                        if (row == null)
                        {
                            return Result.Fail(Data.NotFound);
                        }
                        List<string> cache = new List<string>();
                        foreach (var item in row.BasicInstruments)
                        {
                            item.BasicInstrumentTemplateId = null;
                            await Repository.UpdateAsync(item);
                            cache.AddRange(StaticClass.Instruments.Cache.Key(item.Id, item.ProjectId));
                        }

                        await Repository.RemoveAsync(row);
                        cache.Add(StaticClass.InstrumentTemplates.Cache.GetAll);
                        var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                        return Result.EndPointResult(result, Data.Succesfully, Data.Fail);
                    }
                    catch (Exception ex)
                    {
                        string esm = ex.Message;
                        // Log the exception (logging mechanism not shown here)
                        return Result.Fail("An error occurred while processing the request.");
                    }
                });
            }
        }
    }
}
