using Server.EndPoint.Brands.Queries;
using Server.EndPoint.Templates.Pipes.Queries;
using Server.ExtensionsMethods.Pipings;
using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Enums.NozzleTypes;
using Shared.Models.Templates.NozzleTemplates;
using Shared.Models.Templates.Pipings.Records;
using Shared.Models.Templates.Pipings.Responses;

namespace Server.EndPoint.Templates.Pipes.Queries
{
    public static class GetPipeTemplateByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PipeTemplates.EndPoint.GetById,
                    async (GetPipeTemplateByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<PipeTemplate>, IIncludableQueryable<PipeTemplate, object>> Includes = x =>x
            
                    .Include(x => x.BrandTemplate!);


                    Expression<Func<PipeTemplate, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.PipeTemplates.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria, Includes: Includes);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map();
                    return Result.Success(response);

                });
            }
        }


       
    }
}
