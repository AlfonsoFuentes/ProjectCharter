using Shared.Models.Templates.Equipments.Requests;

namespace Server.EndPoint.Templates.Equipments.Commands
{
    public static class DeleteEquipmentTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.EquipmentTemplates.EndPoint.Delete, async (DeleteEquipmentTemplateRequest Data, IRepository Repository) =>
                {
                    Func<IQueryable<EquipmentTemplate>, IIncludableQueryable<EquipmentTemplate, object>> Includes = x => x
                    .Include(x => x.BasicEquipments);
                    Expression<Func<EquipmentTemplate, bool>> Criteria = x => x.Id == Data.Id;

                    var row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);

                    if (row == null) { return Result.Fail(Data.NotFound); }
                    List<string> cache = new List<string>();
                    foreach (var item in row.BasicEquipments)
                    {
                        item.BasicEquipmentTemplateId = null;
                        await Repository.UpdateAsync(item);
                        cache.AddRange(StaticClass.BasicEquipments.Cache.Key(item.Id, item.ProjectId));
                    }
                    await Repository.RemoveAsync(row);
                    cache.Add(StaticClass.EquipmentTemplates.Cache.GetAll);
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }

}
