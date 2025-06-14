namespace Server.EndPoint.StakeHolders.Commands
{
    public static class CreateUpdateStakeHolderEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.StakeHolders.EndPoint.CreateUpdate, async (StakeHolderResponse Data, IRepository Repository) =>
                {

                    StakeHolder? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        row = StakeHolder.Create();
                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<StakeHolder>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }
                    
                   
                    Data.Map(row);
                    List<string> cache = [ .. StaticClass.StakeHolders.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


      public  static StakeHolder Map(this StakeHolderResponse request, StakeHolder row)
        {
            row.Name = request.Name;
            row.Email = request.Email;
            row.PhoneNumber = request.PhoneNumber;
            row.Area = request.Area;

            return row;
        }

    }

}
