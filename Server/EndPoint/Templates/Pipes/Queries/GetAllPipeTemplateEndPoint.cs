
namespace Server.EndPoint.Templates.Pipes.Queries
{
    public static class GetAllPipeTemplateEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PipeTemplates.EndPoint.GetAll, async (PipeTemplateGetAll request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<PipeTemplate>, IIncludableQueryable<PipeTemplate, object>> Includes = x => x
               
                    .Include(x => x.BrandTemplate!)
                     ;

                    string CacheKey = StaticClass.PipeTemplates.Cache.GetAll;
                    var rows = await Repository.GetAllAsync(CacheKey, Includes: Includes);

                    if (rows == null)
                    {
                        return Result<PipeTemplateResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.PipeTemplates.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    PipeTemplateResponseList response = new PipeTemplateResponseList()
                    {
                        Items = maps
                    };
                    return Result<PipeTemplateResponseList>.Success(response);

                });
            }
        }
    }
}