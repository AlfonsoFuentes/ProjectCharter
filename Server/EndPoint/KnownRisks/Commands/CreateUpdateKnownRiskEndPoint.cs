namespace Server.EndPoint.KnownRisks.Commands
{

    public static class CreateUpdateKnownRiskEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.KnownRisks.EndPoint.CreateUpdate, async (KnownRiskResponse Data, IRepository Repository) =>
                {

                    KnownRisk? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<KnownRisk, Project>(Data.ProjectId);

                        row = KnownRisk.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<KnownRisk>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);


                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });
            }
            private string[] GetCacheKeys(KnownRisk row)
            {
                List<string> cacheKeys = [
                    .. StaticClass.KnownRisks.Cache.Key(row.Id, row.ProjectId)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }


        static KnownRisk Map(this KnownRiskResponse request, KnownRisk row)
        {
            row.Name = request.Name;
            return row;
        }

    }

}
