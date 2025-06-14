using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Validators;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Validators;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicPipes.Validators
{
    public static class ValidateBasicPipeTagsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicPipes.EndPoint.ValidateTag, async (ValidateBasicPipeTagRequest Data, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> Includes = x => x
                      .Include(x => x.BasicEngineeringItems)
                      ;
                    Expression<Func<Project, bool>> CriteriaDeliverable = x => x.Id == Data.ProjectId;
                    string CacheKey = StaticClass.Projects.Cache.GetById(Data.ProjectId);

                    var project = await Repository.GetAsync(Cache: CacheKey, Criteria: CriteriaDeliverable, Includes: Includes);
                    if (project == null) return false;
                    var basicengineeringitems = project.BasicEngineeringItems.OfType<BasicPipeItem>();
                    if (basicengineeringitems == null) return false;


                    Func<BasicPipeItem, bool> CriteriaExist = x => Data.Id == null ?
                    x.Tag.Equals(Data.Tag) : x.Id != Data.Id.Value && x.Tag.Equals(Data.Tag);

                    var result = basicengineeringitems.Any(x => CriteriaExist(x));

                    return result;
                });


            }
        }



    }

}
