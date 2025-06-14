using Shared.Models.Templates.Pipings.Validators;

namespace Server.EndPoint.Templates.Pipes.Validators
{
    public static class ValidatePipeTemplatesNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.PipeTemplates.EndPoint.Validate, async (ValidatePipeTemplateRequest Data, IQueryRepository Repository) =>
                {
                    Func<IQueryable<PipeTemplate>, IIncludableQueryable<PipeTemplate, object>> Includes = x => x
                    .Include(x => x.BrandTemplate!)
                     ;


                    Expression<Func<PipeTemplate, bool>> CriteriaId = null!;
                    Func<PipeTemplate, bool> CriteriaExist = x => Data.Id == null ?
                    x.Material.Equals(Data.Material) &&
                 
                    x.Insulation == Data.Insulation &&
                    x.Class.Equals(Data.Class) &&
                    x.Diameter.Equals(Data.Diameter)
                    : x.Id != Data.Id.Value &&
                     x.Material.Equals(Data.Material) &&
          
                    x.Insulation == Data.Insulation &&
                    x.Class.Equals(Data.Class) &&
                    x.Diameter.Equals(Data.Diameter);

                    string CacheKey = StaticClass.PipeTemplates.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId, Includes: Includes);
                });


            }
        }



    }

}
