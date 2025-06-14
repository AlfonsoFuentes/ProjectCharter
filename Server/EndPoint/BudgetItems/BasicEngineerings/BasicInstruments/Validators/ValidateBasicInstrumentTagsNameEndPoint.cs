using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Validators;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicInstruments.Validators
{
    public static class ValidateBasicInstrumentTagsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicInstruments.EndPoint.ValidateTag, async (ValidateBasicInstrumentTagRequest Data, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> Includes = x => x
                       .Include(x => x.BasicEngineeringItems)
                       ;
                    Expression<Func<Project, bool>> CriteriaDeliverable = x => x.Id == Data.ProjectId;
                    string CacheKey = StaticClass.Projects.Cache.GetById(Data.ProjectId);

                    var project = await Repository.GetAsync(Cache: CacheKey, Criteria: CriteriaDeliverable, Includes: Includes);
                    if (project == null) return false;
                    var basicEngineeringItems = project.BasicEngineeringItems.OfType<BasicInstrumentItem>();
                    if (basicEngineeringItems == null) return false;


                    Func<BasicInstrumentItem, bool> CriteriaExist = x => Data.Id == null ?
                    x.Tag.Equals(Data.Tag) : x.Id != Data.Id.Value && x.Tag.Equals(Data.Tag);

                    var result = basicEngineeringItems.Any(x => CriteriaExist(x));

                    return result;
                });


            }
        }



    }

}
