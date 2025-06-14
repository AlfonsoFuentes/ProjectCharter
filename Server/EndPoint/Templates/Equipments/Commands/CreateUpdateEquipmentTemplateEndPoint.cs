using Server.ExtensionsMethods.InstrumentTemplateMapper;
using Shared.Models.Templates.Equipments.Responses;
namespace Server.EndPoint.Templates.Equipments.Commands
{

    public static class CreateUpdateEquipmentTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.EquipmentTemplates.EndPoint.CreateUpdate, async (EquipmentTemplateResponse Data, IRepository Repository) =>
                {
                    EquipmentTemplate? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = Template.AddEquipmentTemplate();

                        await Repository.AddAsync(row);
                        await NozzleMapper.CreateNozzleTemplates(Repository, row.Id, Data.Nozzles);
                    }
                    else
                    {
                        Expression<Func<EquipmentTemplate, bool>> Criteria = x => x.Id == Data.Id;
                        Func<IQueryable<EquipmentTemplate>, IIncludableQueryable<EquipmentTemplate, object>> Includes = x => x
                        .Include(x => x.BrandTemplate!)
                        .Include(x => x.NozzleTemplates)
                        .Include(x => x.BasicEquipments)
                        ;
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
            private string[] GetCacheKeys(EquipmentTemplate row)
            {


                var templates = StaticClass.EquipmentTemplates.Cache.Key(row.Id);
                var items = row.BasicEquipments == null ? new[] { string.Empty } : row.BasicEquipments.Select(x => StaticClass.BasicEquipments.Cache.GetById(x.Id)).ToArray();
                List<string> cacheKeys = [
                        ..items,
                        ..templates

                ];

                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
