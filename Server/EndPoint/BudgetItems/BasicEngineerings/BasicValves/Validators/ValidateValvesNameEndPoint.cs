using Server.Database.Entities.BudgetItems.Commons;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Validators;
using Shared.Models.BudgetItems.IndividualItems.Valves.Validators;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicValves.Validators
{
    public static class ValidateBasicValvesNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicValves.EndPoint.Validate, async (ValidateBasicValveRequest Data, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> Includes = x => x
                      .Include(x => x.BasicEngineeringItems)
                      ;
                    Expression<Func<Project, bool>> CriteriaDeliverable = x => x.Id == Data.ProjectId;
                    string CacheKey = StaticClass.Projects.Cache.GetById(Data.ProjectId);

                    var project = await Repository.GetAsync(Cache: CacheKey, Criteria: CriteriaDeliverable, Includes: Includes);
                    if (project == null) return false;
                    var basicengineeringitems = project.BasicEngineeringItems.OfType<BasicValveItem>();
                    if (basicengineeringitems == null) return false;

                    Func<BasicValveItem, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);

                    var result = basicengineeringitems.Any(x => CriteriaExist(x));

                    return result;
                });


            }
        }



    }

}
