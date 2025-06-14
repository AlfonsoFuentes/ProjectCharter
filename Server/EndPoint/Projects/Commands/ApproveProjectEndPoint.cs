using Server.Database.Entities;

namespace Server.EndPoint.Projects.Commands
{
    public static class ApproveProjectEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Projects.EndPoint.Approve, async (ApproveProjectRequest Data, IRepository Repository, IQueryRepository queryRepository) =>
                {
                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> Includes = x => x
                        .Include(x => x.BudgetItems)
                        .Include(p => p.BudgetItems).ThenInclude(x => (x as Instrument)!.InstrumentItems!).ThenInclude(x => x.InstrumentTemplate!)
                        .Include(p => p.BudgetItems).ThenInclude(x => (x as Pipe)!.PipeItems!).ThenInclude(x => x.PipeTemplate)
                        .Include(p => p.BudgetItems).ThenInclude(x => (x as Valve)!.ValveItems!).ThenInclude(x => x.ValveTemplate!)
                        .Include(p => p.BudgetItems).ThenInclude(x => (x as Equipment)!.EquipmentItems!).ThenInclude(x => x.EquipmentTemplate!)
                     ;

                    Expression<Func<Project, bool>> Criteria = x => x.Id == Data.Id;

                    var row = await Repository.GetAsync(Criteria: Criteria, Includes: Includes);

                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.UpdateAsync(row);
                    Data.Map(row);
                    double totalpercentage = Data.PercentageContingency + Data.PercentageEngineering;

                    if (100 - totalpercentage > 0)
                    {
                        var contingency = row.Contingencys.FirstOrDefault();
                        if (contingency != null)
                        {
                            contingency.BudgetUSD = Math.Round(row.CapitalBudgetUSD /
                            (100 - totalpercentage) * contingency.Percentage, 1);
                            await Repository.UpdateAsync(contingency);
                        }
                        var engineering = row.EngineeringSalaries.FirstOrDefault();
                        if (engineering != null)
                        {
                            engineering.BudgetUSD = Math.Round(row.CapitalBudgetUSD /
                           (100 - totalpercentage) * engineering.Percentage, 1);
                            await Repository.UpdateAsync(engineering);
                        }
                    }

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(StaticClass.Projects.Cache.Key(row.Id));


                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }

        }

        static Project Map(this ApproveProjectRequest request, Project row)
        {
            row.Name = request.Name;

            row.StartDate = request.InitialProjectDate == null ? null : request.InitialProjectDate.Value;

            row.ProjectNumber = request.ProjectNumber;
            row.PercentageEngineering = request.PercentageEngineering;
            row.PercentageContingency = request.PercentageContingency;
            row.PercentageTaxProductive = request.PercentageTaxProductive;
            row.IsProductiveAsset = request.IsProductiveAsset;
            row.ProjectNeedType = request.ProjectNeedType.Id;
            row.CostCenter = request.CostCenter.Id;
            row.Focus = request.Focus.Id;
            row.Status = ProjectStatusEnum.Approved.Id;
            return row;
        }


    }

}
