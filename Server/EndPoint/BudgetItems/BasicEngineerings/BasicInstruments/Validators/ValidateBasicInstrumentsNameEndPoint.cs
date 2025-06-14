using Server.Database.Entities.BudgetItems.Commons;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Validators;
using Shared.Models.BudgetItems.IndividualItems.Instruments.Validators;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicInstruments.Validators
{
    public static class ValidateBasicInstrumentsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicInstruments.EndPoint.Validate, async (ValidateBasicInstrumentRequest Data, IQueryRepository Repository) =>
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
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);

                    var result = basicEngineeringItems.Any(x => CriteriaExist(x));

                    return result;
                });


            }
        }



    }

}
