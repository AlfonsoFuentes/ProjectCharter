namespace Server.EndPoint.Bennefits.Commands
{

    public static class CreateUpdateBennefitEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Bennefits.EndPoint.CreateUpdate, async (BennefitResponse Data, IRepository Repository) =>
                {

                    Bennefit? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<Bennefit, Project>(Data.ProjectId);

                        row = Bennefit.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<Bennefit>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }

                    Data.Map(row);
                    List<string> cache = [.. StaticClass.Bennefits.Cache.Key(row.Id, row.ProjectId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        static Bennefit Map(this BennefitResponse request, Bennefit row)
        {
            row.Name = request.Name;
            return row;
        }

    }

}
