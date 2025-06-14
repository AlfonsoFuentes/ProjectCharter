using Shared.Models.EngineeringFluidCodes.Requests;

namespace Server.EndPoint.EngineeringFluidCodes.Commands
{
    public static class DeleteGroupEngineeringFluidCodeEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.EngineeringFluidCodes.EndPoint.DeleteGroup, async (DeleteGroupEngineeringFluidCodeRequest Data, IRepository Repository) =>
                {
                    Func<IQueryable<EngineeringFluidCode>, IIncludableQueryable<EngineeringFluidCode, object>>
                     Includes = x => x.Include(x => x.BasicPipeItems);

                    foreach (var item in Data.SelecteItems)
                    {
                        Expression<Func<EngineeringFluidCode, bool>> Criteria = x => x.Id == item.Id;
                        var row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);
                        if(row!=null)
                        {
                            foreach (var cases in row.BasicPipeItems)
                            {
                                cases.BasicFluidCodeId = null;
                                await Repository.UpdateAsync(cases);
                            }

                            await Repository.RemoveAsync(row);
                        }
                        
                    }

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(StaticClass.EngineeringFluidCodes.Cache.GetAll);

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }

}
