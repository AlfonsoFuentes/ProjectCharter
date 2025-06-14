using Shared.Models.Templates.Valves.Responses;

namespace Server.EndPoint.Templates.Valves.Commands
{

    public static class CreateUpdateValveTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ValveTemplates.EndPoint.CreateUpdate, async (ValveTemplateResponse Data, IRepository Repository) =>
                {
                    ValveTemplate? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        row = Template.AddValveTemplate();
                        await Repository.AddAsync(row);
                        await NozzleMapper.CreateNozzleTemplates(Repository, row.Id, Data.Nozzles);
                    }
                    else
                    {
                        Func<IQueryable<ValveTemplate>, IIncludableQueryable<ValveTemplate, object>> Includes = x => x
                           .Include(x => x.NozzleTemplates)
                           .Include(x => x.BrandTemplate!)
                           .Include(x => x.BasicValves)
                           ;

                        Expression<Func<ValveTemplate, bool>> Criteria = x => x.Id == Data.Id;

                        row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                        await NozzleMapper.UpdateNozzlesTemplate(Repository, row.NozzleTemplates, Data.Nozzles, row.Id);
                    }
                    Data.Map(row);



                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
            private string[] GetCacheKeys(ValveTemplate row)
            {


                var templates = StaticClass.ValveTemplates.Cache.Key(row.Id);
                var items = row.BasicValves == null ? new[] { string.Empty } : row.BasicValves.Select(x => StaticClass.BasicValves.Cache.GetById(x.Id)).ToArray();
                List<string> cacheKeys = [
                        ..items,
                        ..templates

                ];

                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
