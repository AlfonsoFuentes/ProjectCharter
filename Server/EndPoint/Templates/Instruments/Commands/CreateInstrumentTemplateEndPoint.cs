
using Server.EndPoint.Templates.Instruments.Queries;
using Server.ExtensionsMethods.InstrumentTemplateMapper;
using Shared.Models.Templates.Instruments.Responses;

namespace Server.EndPoint.Templates.Instruments.Commands
{

    public static class CreateUpdateInstrumentTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InstrumentTemplates.EndPoint.CreateUpdate, async (InstrumentTemplateResponse Data, IRepository Repository) =>
                {
                    InstrumentTemplate? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = Template.AddInstrumentTemplate();

                        await Repository.AddAsync(row);
                        await NozzleMapper.CreateNozzleTemplates(Repository, row.Id, Data.Nozzles);
                    }
                    else
                    {
                        Expression<Func<InstrumentTemplate, bool>> Criteria = x => x.Id == Data.Id;
                        Func<IQueryable<InstrumentTemplate>, IIncludableQueryable<InstrumentTemplate, object>> Includes = x => x
                        .Include(x => x.BrandTemplate!)
                        .Include(x => x.NozzleTemplates)
                        .Include(x => x.BasicInstruments);
                         row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);

                        if (row == null) { return Result.Fail(Data.NotFound); }

                        await Repository.UpdateAsync(row);
                        await NozzleMapper.UpdateNozzlesTemplate(Repository, row.NozzleTemplates, Data.Nozzles, row.Id);
                    }
                    

                    Data.Map(row);



                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result, Data.Succesfully,
                        Data.Fail);


                });


            }
            private string[] GetCacheKeys(InstrumentTemplate row)
            {


                var templates = StaticClass.InstrumentTemplates.Cache.Key(row.Id);
                var items = row.BasicInstruments == null ? new[] { string.Empty } : row.BasicInstruments.Select(x => StaticClass.BasicInstruments.Cache.GetById(x.Id)).ToArray();
                List<string> cacheKeys = [
                        ..items,
                        ..templates

                ];

                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
