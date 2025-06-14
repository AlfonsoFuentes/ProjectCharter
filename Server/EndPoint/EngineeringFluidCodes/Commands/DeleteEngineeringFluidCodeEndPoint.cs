



using Server.Database.Entities;
using Shared.Models.EngineeringFluidCodes.Requests;

namespace Server.EndPoint.EngineeringFluidCodes.Commands
{
    public static class DeleteEngineeringFluidCodeEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.EngineeringFluidCodes.EndPoint.Delete, async (DeleteEngineeringFluidCodeRequest Data, IRepository Repository) =>
                {
                    Func<IQueryable<EngineeringFluidCode>, IIncludableQueryable<EngineeringFluidCode, object>>
                     Includes = x => x.Include(x => x.BasicPipeItems);

                    Expression<Func<EngineeringFluidCode, bool>> Criteria = x => x.Id == Data.Id;


                    var row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);


                    if (row == null) { return Result.Fail(Data.NotFound); }

                    foreach (var cases in row.BasicPipeItems)
                    {
                        cases.BasicFluidCodeId = null;
                        await Repository.UpdateAsync(cases);
                    }

                    await Repository.RemoveAsync(row);

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(StaticClass.EngineeringFluidCodes.Cache.Key(row.Id));

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }

}
