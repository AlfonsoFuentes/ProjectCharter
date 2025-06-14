using Shared.Models.BudgetItems.IndividualItems.Taxs.Records;
using Shared.Models.BudgetItems.IndividualItems.Taxs.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.Taxs.Queries
{
    public static class GetBudgetItemToApplyTaxesEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Taxs.EndPoint.GetBudgetItemsToApplyTaxById, async (GetBudgetItemsToApplyTaxRequest request, IQueryRepository Repository) =>
                {

                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> Includes = x => x.Include(x => x.BudgetItems);

                    Expression<Func<Project, bool>> Criteria = x => x.Id == request.ProjectId;

                    string CacheKey = StaticClass.Projects.Cache.GetById(request.ProjectId);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria, Includes: Includes);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }


                    var items = row.BudgetItems.Where(x =>
                        x.GetType() == typeof(Structural) ||
                        x.GetType() == typeof(Foundation) ||
                        x.GetType() == typeof(Equipment) ||
                        x.GetType() == typeof(Electrical) ||
                        x.GetType() == typeof(Pipe) ||
                        x.GetType() == typeof(Valve) ||
                        x.GetType() == typeof(Instrument) ||
                        x.GetType() == typeof(EHS) ||
                        x.GetType() == typeof(Painting) ||
                        x.GetType() == typeof(Testing) ||
                        x.GetType() == typeof(EngineeringDesign)
                        ).Select(x => x.Map()).ToList();



                    TaxItemResponseList response = new TaxItemResponseList()
                    {
                        Items = items,


                    };
                    return Result<TaxItemResponseList>.Success(response);


                });
            }


        }
        static TaxItemResponse Map(this BudgetItem row)
        {
            return new()
            {
                Budget = row.BudgetUSD,
                BudgetItemId = row.Id,

                Name = row.Name,
                Nomenclatore = row.Nomenclatore,
            };

        }


    }
}
