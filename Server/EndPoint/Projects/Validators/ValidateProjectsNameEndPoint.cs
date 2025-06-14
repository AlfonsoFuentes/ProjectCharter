using Shared.Models.Projects.Validators;
using Shared.Models.Backgrounds.Validators;

namespace Server.EndPoint.Projects.Validators
{
    public static class ValidateProjectsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Projects.EndPoint.Validate, async (ValidateProjectRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Project, bool>> CriteriaId = null!;
                    Func<Project, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.Projects.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }
}
