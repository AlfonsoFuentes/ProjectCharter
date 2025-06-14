using Server.ExtensionsMethods.Projects;
using Shared.Models.BudgetItems.IndividualItems.Alterations.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.Alterations.Commands
{
    public static class CreateUpdateAlterationEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Alterations.EndPoint.CreateUpdate, async (AlterationResponse data, IRepository repository) =>
                {
                    var project = await ProjectMapper.GetProject(data.ProjectId, repository);
                    if (project == null) return Result.Fail(data.Fail);

                    Alteration? row = null!;
                    if (data.Id == Guid.Empty)
                    {
                        int order = ProjectMapper.GetNextOrder<Alteration>(project);
                        row = Alteration.Create(project.Id);
                        row.Order = order;
                        await repository.AddAsync(row);
                    }
                    else
                    {
                        row = await repository.GetByIdAsync<Alteration>(data.Id);
                        if (row == null) return Result.Fail(data.NotFound);

                        await repository.UpdateAsync(row);
                    }


                    data.Map(row);


                    var result = await repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));

                    return Result.EndPointResult(result, data.Succesfully, data.Fail);
                });
            }
            private string[] GetCacheKeys(BudgetItem row)
            {
               
                var budgetitems = StaticClass.BudgetItems.Cache.Key(row.Id, row.ProjectId);
                var items = StaticClass.Alterations.Cache.Key(row.Id, row.ProjectId);

                List<string> cacheKeys = [
                     ..budgetitems,
                     ..items,
              

                ];

                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }

        }

        static Alteration Map(this AlterationResponse request, Alteration alteration)
        {
            alteration.Name = request.Name;
            alteration.UnitaryCost = request.UnitaryCost;
            alteration.CostCenter = request.CostCenter.Name;
            alteration.Quantity = request.Quantity;
            alteration.BudgetUSD = request.BudgetUSD;
            alteration.IsAlteration = true;
            return alteration;
        }
    }
}

