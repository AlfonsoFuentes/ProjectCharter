namespace Server.EndPoint.Requirements.Commands
{

    public static class CreateUpdateRequirementEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Requirements.EndPoint.CreateUpdate, async (RequirementResponse Data, IRepository Repository) =>
                {
                    Requirement? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<Requirement, Project>(Data.ProjectId);

                        row = Requirement.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<Requirement>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }




                    Data.Map(row);
                    List<string> cache = [.. StaticClass.Requirements.Cache.Key(row.Id, row.ProjectId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
          
            private string[] GetCacheKeys(Requirement row)
            {
                List<string> cacheKeys = [
                    .. StaticClass.Requirements.Cache.Key(row.Id, row.ProjectId)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }


        static Requirement Map(this RequirementResponse request, Requirement row)
        {
            row.Name = request.Name;
            return row;
        }

    }

}
