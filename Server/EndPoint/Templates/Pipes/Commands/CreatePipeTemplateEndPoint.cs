namespace Server.EndPoint.Templates.Pipes.Commands
{

    public static class CreateUpdatePipeTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PipeTemplates.EndPoint.CreateUpdate, async (PipeTemplateResponse Data, IRepository Repository) =>
                {
                    PipeTemplate? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = Template.AddPipeTemplate();

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        Expression<Func<PipeTemplate, bool>> Criteria = x => x.Id == Data.Id;
                        Func<IQueryable<PipeTemplate>, IIncludableQueryable<PipeTemplate, object>> Includes = x => x
                        .Include(x => x.BasicPipeItems);

                        row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);

                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);

                        Data.Map(row);
                    }


                    Data.Map(row);



                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
            private string[] GetCacheKeys(PipeTemplate row)
            {


                var templates = StaticClass.PipeTemplates.Cache.Key(row.Id);
                //var items = row.BasicPipeItems == null ? new[] { string.Empty } : row.BasicPipeItems.Select(x => StaticClass.BasicPipes.Cache.GetById(x.Id)).ToArray();
                List<string> cacheKeys = [
                        //..items,
                        ..templates

                ];

                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }




    }

}
